using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableDoor : MonoBehaviour
{
    private Color _color;
    public Renderer[] _renderers;

    void start()
    {
        _color = Color.white;
    }

    void Update()
    {
        var health = GetComponent<Health>();
        _color.r = 2 - health.currentHP / health.initialHP;
        _color.g = health.currentHP / health.initialHP;
        _color.b = health.currentHP / health.initialHP;
        foreach (Renderer rend in _renderers)
        {
            rend.material.SetColor(rend.material.shader.name, _color);
        }

    }
}
