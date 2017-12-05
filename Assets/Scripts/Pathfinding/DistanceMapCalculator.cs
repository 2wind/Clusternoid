using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DistanceMapCalculator
{
    static Texture2D prevTarget;
    static Color[] pixels;
    static int[] result;
    static Color[] resultPixels;
    public static IEnumerator CalculateFlowMap(Texture2D target, Texture2D render, Vector2Int start)
    {
        var watch = new Stopwatch();
        watch.Start();
        if (prevTarget == null || prevTarget != target)
        {
            pixels = target.GetPixels();
            result = new int[pixels.Length];
            resultPixels = new Color[pixels.Length];
        }
        prevTarget = target;
        var width = target.width;
        var height = target.height;
        var distTask = Task.Run(() => CalculateDistanceMapAsync(pixels, width, height, start, result));
        var flowTask = distTask.ContinueWith(dmap => CalculateFlowMapAsync(width, height, dmap.Result, resultPixels));
        while (!flowTask.IsCompleted) yield return null;
        Debug.Log(watch.Elapsed);
        render.SetPixels(flowTask.Result);
        render.Apply();
    }

    static int[] CalculateDistanceMapAsync(Color[] map, int width, int height, Vector2Int start, int[] result)
    {
        Func<int, Vector2Int> I2V = i => Index2Vec(width, height, i);
        Func<Vector2Int, int> V2I = v => Vec2Index(width, height, v);
        var distMap = result;
        for (var i = 0; i < distMap.Length; i++) distMap[i] = 0;
        var startPoints = new List<Vector2Int> { start, start + Vector2Int.right, start + Vector2Int.up, start + Vector2Int.one };
        foreach (var v in startPoints) distMap[V2I(v)] = 1;
        return Calculate(I2V, V2I, distMap, startPoints, new List<Vector2Int>(), map);
    }

    static Color[] CalculateFlowMapAsync(int width, int height, int[] distanceMap, Color[] result)
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                SetFlowMapPoint(x, y, width, height, distanceMap, result);
            }
        }
        return result;
    }

    static void SetFlowMapPoint(int x, int y, int width, int height, int[] distanceMap, Color[] result)
    {
        var center = distanceMap[Vec2Index(x, y, width, height)];
        if (center == 0 || center == 1)
        {
            result[Vec2Index(x, y, width, height)] = Color.black;
            return;
        }
        var left = InRange(x - 1, y, width, height) ? distanceMap[Vec2Index(x - 1, y, width, height)] : 0;
        var right = InRange(x + 1, y, width, height) ? distanceMap[Vec2Index(x + 1, y, width, height)] : 0;
        var up = InRange(x, y + 1, width, height) ? distanceMap[Vec2Index(x, y + 1, width, height)] : 0;
        var down = InRange(x, y - 1, width, height) ? distanceMap[Vec2Index(x, y - 1, width, height)] : 0;
        Func<int, int> GetInt = i => i == 0 ? center : i;
        var dx = GetInt(left) - GetInt(right);
        var dy = GetInt(down) - GetInt(up);
        result[Vec2Index(x, y, width, height)] = new Color(dy * 0.25f + 0.5f, Mathf.Clamp(dx * 0.5f, 0f, 1f), Mathf.Clamp(-dx * 0.5f, 0f, 1f), 1);
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
        return width * y + x;
    }
    static Vector2Int Index2Vec(int width, int height, int index)
        => new Vector2Int(index % width, index / width);

    static int[] Calculate(
        Func<int, Vector2Int> i2v,
        Func<Vector2Int, int> v2i,
        int[] prevMap,
        List<Vector2Int> np1,
        List<Vector2Int> np2,
        Color[] map)
    {
        if (np1.Count == 0) return prevMap;
        foreach (var point in np1)
        {
            AddPoint(i2v, v2i, prevMap, np2, new Vector2Int(point.x, point.y + 1), point, map);
            AddPoint(i2v, v2i, prevMap, np2, new Vector2Int(point.x, point.y - 1), point, map);
            AddPoint(i2v, v2i, prevMap, np2, new Vector2Int(point.x + 1, point.y), point, map);
            AddPoint(i2v, v2i, prevMap, np2, new Vector2Int(point.x - 1, point.y), point, map);
        }
        np1.Clear();
        return Calculate(i2v, v2i, prevMap, np2, np1, map);
    }

    static void AddPoint(Func<int, Vector2Int> i2v, Func<Vector2Int, int> v2i, int[] prevMap, List<Vector2Int> np, Vector2Int target, Vector2Int point, Color[] map)
    {
        try
        {
            var index = v2i(target);
            if (prevMap[index] != 0 || map[index].r < 0.5f) return;
            prevMap[index] = prevMap[v2i(point)] + 1;
            np.Add(target);
        }
        catch { return; }
    }
}