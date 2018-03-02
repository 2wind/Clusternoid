using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableDoor : MonoBehaviour
{
    private Color _color;
    public Renderer[] renderers;
    private HealthBar _healthBar;
    private Vector3 _healthBarPosition;


    void Start()
    {
        _color = Color.white;
    }

    void OnEnable()
    {
        _healthBarPosition = transform.position + Vector3.back * 4;
        _healthBar = UIPool.Get("HealthBar").GetComponent<HealthBar>();
        _healthBar.FollowPosition = _healthBarPosition;
        _healthBar.health = GetComponent<Health>();
    }

    void Update()
    {
        var health = GetComponent<Health>();
        _color.r = 2 - health.currentHP / health.initialHP;
        _color.g = health.currentHP / health.initialHP;
        _color.b = health.currentHP / health.initialHP;
        foreach (Renderer rend in renderers)
        {
            rend.material.SetColor(rend.material.shader.name, _color);
        }

    }

    void OnDisable()
    {
        if (_healthBar != null)
        {
            _healthBar.transform.SetParent(UIPool.instance.transform);
            _healthBar.gameObject.SetActive(false);
        }
    }
}
