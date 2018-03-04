using System.Collections;
using UnityEngine;

public class PathFinder : Singleton<PathFinder>
{
    public Texture2D map;
    public Transform target;
    Color[] resultPixels;
    bool calculated;

    IEnumerator Start()
    {
        if (map == null) yield break;
        resultPixels = new Color[map.width * map.height];
        while (target == null) yield return null;
        while (true)
        {
            var targetPosition = WorldToPixelSpace(target.position);
            yield return
                StartCoroutine(DistanceMapCalculator.CalculateFlowMap(map, null, targetPosition, resultPixels));
            calculated = true;
        }
    }

    Vector2Int WorldToPixelSpace(Vector2 targetPosition)
    {
        var center = new Vector2(map.width / 2f, map.height / 2f);
        return new Vector2Int(Mathf.FloorToInt(targetPosition.x + center.x),
            Mathf.RoundToInt(targetPosition.y + center.y));
    }

    // 일부 벽을 외부로 판정하는 버그가 있으므로, 특히 Update()에서 돌리지 말 것.
    public static bool IsInMap(Vector2 position)
    {
        var pos = instance.transform.InverseTransformPoint(position);
        var pixelPos = instance.WorldToPixelSpace(pos);
        var map = instance.map;

        if (map.GetPixel(pixelPos.x, pixelPos.y).Equals(Color.black))
        {
            return false;
        }

        return pixelPos.x >= 0 && pixelPos.x < map.width
               && pixelPos.y >= 0 && pixelPos.y <= map.width;
    }

    public static Vector2 GetAcceleration(Vector2 position)
    {
        if (!instance.calculated) return Vector2.zero;
        if (!IsInMap(position)) return ToTarget(position);
        var center = new Vector2(instance.map.width / 2f, instance.map.height / 2f);
        var localPos = instance.transform.InverseTransformPoint(position);
        var x = Mathf.RoundToInt(localPos.x + center.x);
        var y = Mathf.RoundToInt(localPos.y + center.y);
        var color = instance.resultPixels[x + y * instance.map.width];
        return -ColorToAccel(color).normalized;
    }

    static Vector2 ToMapCenter(Vector2 position)
    {
        Vector2 pos = instance.transform.InverseTransformPoint(position);
        var center = new Vector2(instance.map.width / 2f, instance.map.height / 2f);
        return (center - pos).normalized;
    }

    static Vector2 ToTarget(Vector2 position)
    {
        Vector2 pos = instance.transform.InverseTransformPoint(position);
        return ((Vector2)instance.target.position - pos).normalized;
    }
    
    static Vector2 ColorToAccel(Color sample)
        => new Vector2(sample.b - sample.g, 1 - 2 * sample.r);
}