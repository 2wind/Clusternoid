using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTargetPoint : MonoBehaviour
{
    public Material displayed;
    public Texture2D map;
    public Texture2D result;
    Color[] mapPixels;
    [System.NonSerialized]
    public Color[] resultPixels;
    public bool calculated;
    void Awake()
    {
        resultPixels = new Color[map.width * map.height];
    }
    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        if (result == null)
        {
            result = new Texture2D(map.width, map.height);
            result.filterMode = FilterMode.Point;
        }
        if (mapPixels == null) mapPixels = map.GetPixels();
        displayed.mainTexture = result;
        result.SetPixels(mapPixels);
        var point = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var start = new Vector2Int(Mathf.RoundToInt((point.x + 0.5f) * result.width), Mathf.RoundToInt((point.y + 0.5f) * result.height));
        var co = StartCoroutine(DistanceMapCalculator.CalculateFlowMap(map, result, start, resultPixels));
        StartCoroutine(CoroutineEndDetect(co));
    }

    IEnumerator CoroutineEndDetect(Coroutine co)
    {
        yield return co;
        calculated = true;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        displayed.mainTexture = map;
    }
}