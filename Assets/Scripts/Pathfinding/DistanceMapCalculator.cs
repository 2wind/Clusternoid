using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class DistanceMapCalculator
{
    static Texture2D prevTarget;
    static Color[] pixels;
    static float[] result;
    static Color[] resultPixels;

    public static IEnumerator CalculateFlowMap(Texture2D target, Texture2D render, Vector2Int start,
        Color[] returnPixels)
    {
//        var watch = new Stopwatch();
//        watch.Start();
        if (prevTarget == null || prevTarget != target)
        {
            pixels = target.GetPixels();
            result = new float[pixels.Length];
            resultPixels = new Color[pixels.Length];
        }
        prevTarget = target;
        var width = target.width;
        var height = target.height;
        var distTask = Task.Run(() => CalculateDistanceMapAsync(pixels, width, height, start, result));
        var flowTask = distTask.ContinueWith(dmap => CalculateFlowMapAsync(width, height, dmap.Result, resultPixels));
        while (!flowTask.IsCompleted) yield return null;
//        Debug.Log(watch.Elapsed);
        for (var i = 0; i < returnPixels.Length; i++) returnPixels[i] = flowTask.Result[i];
        if (render == null) yield break;
        render.SetPixels(flowTask.Result);
        render.Apply();
    }

    static float[] CalculateDistanceMapAsync(Color[] map, int width, int height, Vector2Int start, float[] resultArray)
    {
        Func<Vector2Int, int> V2I = v => Vec2Index(width, height, v);
        var distMap = resultArray;
        for (var i = 0; i < distMap.Length; i++) distMap[i] = 0;
        var startPoints =
            new List<Vector2Int> {start, start + Vector2Int.right, start + Vector2Int.up, start + Vector2Int.one};
        foreach (var v in startPoints) distMap[V2I(v)] = 1;
        return Calculate(V2I, distMap, startPoints, new List<Vector2Int>(), map);
    }

    static Color[] CalculateFlowMapAsync(int width, int height, float[] distanceMap, Color[] resultArray)
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                SetFlowMapPoint(x, y, width, height, distanceMap, resultArray);
            }
        }
        return resultArray;
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    static void SetFlowMapPoint(int x, int y, int width, int height, float[] distanceMap, Color[] resultArray)
    {
        var center = distanceMap[Vec2Index(x, y, width, height)];
        if (center == 0 || center == 1)
        {
            resultArray[Vec2Index(x, y, width, height)] = Color.black;
            return;
        }
        var left = InRange(x - 1, y, width, height) ? distanceMap[Vec2Index(x - 1, y, width, height)] : 0;
        var right = InRange(x + 1, y, width, height) ? distanceMap[Vec2Index(x + 1, y, width, height)] : 0;
        var up = InRange(x, y + 1, width, height) ? distanceMap[Vec2Index(x, y + 1, width, height)] : 0;
        var down = InRange(x, y - 1, width, height) ? distanceMap[Vec2Index(x, y - 1, width, height)] : 0;
        Func<float, float> GetInt = i => i == 0 ? center + 2f : i;
        var dx = GetInt(left) - GetInt(right);
        var dy = GetInt(down) - GetInt(up);
        resultArray[Vec2Index(x, y, width, height)] = new Color(dy * 0.25f + 0.5f, Mathf.Clamp(dx * 0.5f, 0f, 1f),
            Mathf.Clamp(-dx * 0.5f, 0f, 1f), 1);
    }

    static bool InRange(int x, int y, int width, int height)
    {
        return x < width && x >= 0 && y < height && y >= 0;
    }

    static int Vec2Index(int width, int height, Vector2Int vector)
        => Vec2Index(vector.x, vector.y, width, height);

    static int Vec2Index(int x, int y, int width, int height)
    {
        if (x >= width || x < 0) throw new Exception();
        if (y >= height || y < 0) throw new Exception();
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
        return width * y + x;
    }

    static float[] Calculate(Func<Vector2Int, int> v2i, float[] prevMap, List<Vector2Int> np1, List<Vector2Int> np2,
        Color[] map)
    {
        while (true)
        {
            if (np1.Count == 0) return prevMap;
            foreach (var point in np1)
            {
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x, point.y + 1), point, map);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x, point.y - 1), point, map);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x + 1, point.y), point, map);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x - 1, point.y), point, map);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x + 1, point.y + 1), point, map, 1.4f);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x - 1, point.y + 1), point, map, 1.4f);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x + 1, point.y - 1), point, map, 1.4f);
                AddPoint(v2i, prevMap, np2, new Vector2Int(point.x - 1, point.y - 1), point, map, 1.4f);
            }
            np1.Clear();
            var np3 = np1;
            np1 = np2;
            np2 = np3;
        }
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    static void AddPoint(Func<Vector2Int, int> v2i, float[] prevMap, List<Vector2Int> np, Vector2Int target,
        Vector2Int point, Color[] map, float added = 1)
    {
        try
        {
            var index = v2i(target);
            if (map[index].r < 0.5f) return;
            if (prevMap[index] == 0) np.Add(target);
            var v = prevMap[v2i(point)] + added;
            if (prevMap[index] == 0 || prevMap[index] > v)
                prevMap[index] = v;
        }
        catch
        {
            // ignored
        }
    }
}