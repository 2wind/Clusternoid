using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlinkCharacter : MonoBehaviour
{
    public float blinkTime = 0.1f;
    Renderer[] renderers;
    Material[] originals;
    List<Material> instances;

    float startTime;

    public static void Blink(GameObject go)
    {
        var existing = go.GetComponent<BlinkCharacter>() ?? go.AddComponent<BlinkCharacter>();
        existing.ResetTimer();
    }

    void ResetTimer()
    {
        startTime = Time.time;
    }

    void OnEnable()
    {
        instances = new List<Material>();
        renderers = GetComponentsInChildren<Renderer>();
        originals = renderers.Select(r => r.sharedMaterial).ToArray();
        var groups = renderers.GroupBy(r => r.sharedMaterial);
        foreach (var group in groups)
        {
            var newMat = new Material(group.Key);
            newMat.EnableKeyword("_EMISSION");
            foreach (var meshRenderer in group)
            {
                meshRenderer.sharedMaterial = newMat;
            }
            instances.Add(newMat);
        }
    }

    void Update()
    {
        var timeleft = startTime + blinkTime - Time.time;
        if (timeleft < 0)
        {
            ResetMaterials();
            Destroy(this);
            return;
        }
        var emitColor = Color.red * 2 * (timeleft / blinkTime);
        var color = Color.white * (1 - timeleft / blinkTime);
        foreach (var instance in instances)
        {
            instance.SetColor("_EmissionColor", emitColor);
            instance.color = color;
        }
    }

    void ResetMaterials()
    {
        for (var i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = originals[i];
        }
    }
}