﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Vector3 _followPosition;
    public Health health;
    public RectTransform healthBar;

    public Vector3 FollowPosition
    {
        set
        {
            _followPosition = value;
            transform.position = _followPosition - new Vector3(1.5f, 0);
        }
    }

    // Update is called once per frame
	void Update ()
	{
	    transform.position = _followPosition - new Vector3(1.5f, 0);
        SetHealth(health.initialHP, health.currentHP);
	}

    void SetHealth(int maxHP, int currentHP)
    {
        float ratio = (float) currentHP / maxHP;
        if (ratio < 0) ratio = 0;
        healthBar.localScale = new Vector3(ratio, 1f, 1f);
    }
    
}