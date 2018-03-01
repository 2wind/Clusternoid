using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPool : ObjectPool<UIPool>
{

    public Transform inGameCanvasTransform;


    protected override string Path()
        => "UI/";

    public GameObject GetUI(string target)
    {
        var go = Get(target);
        go.transform.SetParent(inGameCanvasTransform, false);
        return go;
    }
}
