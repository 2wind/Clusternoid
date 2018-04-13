using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class TileLoader : ScriptableObject
{
    public GameObject floor;
    public GameObject wall;
    public GameObject inCorner;
    public GameObject outCorner;
    public GameObject spike;
    public GameObject tar;

    static TileLoader Set => set ?? (set = AssetDatabase.LoadAssetAtPath<TileLoader>("Assets/Tiles.asset"));
    static TileLoader set;

    [MenuItem("Tools/Tileset Loader")]
    static void LoadImage()
    {
        var path = EditorUtility.OpenFilePanel("Open Tileset Image", Application.dataPath, "png");
        if (string.IsNullOrEmpty(path)) return;
        var tex = new Texture2D(2, 2);
        if (path.StartsWith(Application.dataPath))
        {
            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path.Replace(Application.dataPath, "Assets"));
        }
        else
        {
            tex.LoadImage(File.ReadAllBytes(path));
        }
        var pixels = tex.GetPixels32();
        var floors = new List<Vector2Int>();
        var spikes = new List<Vector2Int>();
        var tars = new List<Vector2Int>();
        for (var x = 0; x < tex.width; x++)
        {
            for (var y = 0; y < tex.height; y++)
            {
                var pixel = pixels[x + y * tex.width];
                if (IsSimilarColor(pixel, Color.white)) floors.Add(new Vector2Int(x, y));
                if (IsSimilarColor(pixel, Color.red)) spikes.Add(new Vector2Int(x, y));
                if (IsSimilarColor(pixel, Color.blue)) tars.Add(new Vector2Int(x, y));
            }
        }
        Generate(floors, spikes, tars, new Vector2Int(tex.width, tex.height), tex);
    }

    static bool IsSimilarColor(Color32 one, Color other)
    {
        return Vector4.Magnitude(one - other) < 0.2f;
    }

    static void Generate(List<Vector2Int> floors, List<Vector2Int> spikes, List<Vector2Int> tars, Vector2Int size,
        Texture2D tex)
    {
        var xc = size.x / 2f;
        var yc = size.y / 2f;
        var parent = new GameObject("Tiles");
        var finder = parent.AddComponent<PathFinder>();
        finder.map = tex;
        var outlines = new Dictionary<Vector2Int, Directions>();
        foreach (var floor in floors)
        {
            var go = (GameObject) PrefabUtility.InstantiatePrefab(Set.floor);
            go.transform.position = new Vector2(floor.x - xc, floor.y - yc);
            go.transform.SetParent(parent.transform);
            foreach (var dirc in Directions8().Select((d, i) => Tuple.Create(i, floor - d)).Where(t =>
                !floors.Contains(t.Item2) && !spikes.Contains(t.Item2) && !tars.Contains(t.Item2)))
            {
                if (!outlines.ContainsKey(dirc.Item2))
                    outlines[dirc.Item2] = (Directions) (1 << dirc.Item1);
                else
                    outlines[dirc.Item2] = (Directions) (1 << dirc.Item1) | outlines[dirc.Item2];
            }
        }
        foreach (var spike in spikes)
        {
            var go = (GameObject) PrefabUtility.InstantiatePrefab(Set.spike);
            go.transform.position = new Vector2(spike.x - xc, spike.y - yc);
            go.transform.SetParent(parent.transform);
        }
        foreach (var tar in tars)
        {
            var go = (GameObject) PrefabUtility.InstantiatePrefab(Set.tar);
            go.transform.position = new Vector2(tar.x - xc, tar.y - yc);
            go.transform.SetParent(parent.transform);
        }
        foreach (var kvp in outlines)
        {
            var directions = kvp.Value;
            GameObject original = null;
            var dirc = 0;
            switch ((Directions) ((int) directions % (1 << 4)))
            {
                case Directions.Up:
                    original = Set.wall;
                    dirc = 0;
                    break;
                case Directions.Left:
                    original = Set.wall;
                    dirc = 1;
                    break;
                case Directions.Down:
                    original = Set.wall;
                    dirc = 2;
                    break;
                case Directions.Right:
                    original = Set.wall;
                    dirc = 3;
                    break;
                case Directions.Up | Directions.Left:
                    original = Set.outCorner;
                    dirc = 0;
                    break;
                case Directions.Left | Directions.Down:
                    original = Set.outCorner;
                    dirc = 1;
                    break;
                case Directions.Down | Directions.Right:
                    original = Set.outCorner;
                    dirc = 2;
                    break;
                case Directions.Right | Directions.Up:
                    original = Set.outCorner;
                    dirc = 3;
                    break;
            }
            if (original == null)
            {
                switch (directions)
                {
                    case Directions.DR:
                        original = Set.inCorner;
                        dirc = 2;
                        break;
                    case Directions.LD:
                        original = Set.inCorner;
                        dirc = 1;
                        break;
                    case Directions.UL:
                        original = Set.inCorner;
                        dirc = 0;
                        break;
                    case Directions.RU:
                        original = Set.inCorner;
                        dirc = 3;
                        break;
                    default:
                        continue;
                }
            }
            var go = (GameObject) PrefabUtility.InstantiatePrefab(original);
            go.transform.position = new Vector2(kvp.Key.x - xc, kvp.Key.y - yc);
            go.transform.eulerAngles = new Vector3(0, 0, 90 * dirc);
            go.transform.SetParent(parent.transform);
        }
    }

    static IEnumerable<Vector2Int> Directions8()
    {
        yield return new Vector2Int(0, 1);
        yield return new Vector2Int(-1, 0);
        yield return new Vector2Int(0, -1);
        yield return new Vector2Int(1, 0);
        yield return new Vector2Int(-1, 1);
        yield return new Vector2Int(-1, -1);
        yield return new Vector2Int(1, -1);
        yield return new Vector2Int(1, 1);
    }

    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    enum Directions
    {
        Up = 1,
        Left = 1 << 1,
        Down = 1 << 2,
        Right = 1 << 3,
        UL = 1 << 4,
        LD = 1 << 5,
        DR = 1 << 6,
        RU = 1 << 7
    }
}