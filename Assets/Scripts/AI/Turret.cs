using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI))]
public class Turret : MonoBehaviour
{
    Animator ani;
    Weapon wb;
    AI ai;
    LineRenderer line;
    Rigidbody2D rb;
    public bool isTargetInRange;


    void Start()
    {
        ani = GetComponent<Animator>();
        wb = GetComponent<Weapon>();
        ai = GetComponent<AI>();
        rb = GetComponent<Rigidbody2D>();
        isTargetInRange = false;

        line = wb.firingPosition.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.enabled = false;
    }
    

    void Update()
    {
        ani.SetBool("targetInRange", isTargetInRange);
        if (isTargetInRange)
        {
            CheckFire();
        }
    }


    void CheckFire()
    {
        RaycastHit2D hit = Physics2D.Raycast(wb.firingPosition.position, transform.up, 20, 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            ani.SetBool("targetFound", true);
            rb.freezeRotation = true;
            line.enabled = true;
            line.SetPosition(0, wb.firingPosition.position);
            line.SetPosition(1, hit.point);
        }
        else
        {
            ani.SetBool("targetFound", false);
            rb.freezeRotation = false;
            line.enabled = false;
        }
    }


}