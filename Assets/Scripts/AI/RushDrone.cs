using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingAI))]
public class RushDrone : MonoBehaviour
{

    [HideInInspector]
    public bool isCharging;
    [HideInInspector]
    Melee attackCollider;

    [HideInInspector]
    public MovingAI mAI;

    // Use this for initialization
    void Start()
    {
        mAI = GetComponent<MovingAI>();
        isCharging = false;
        attackCollider = GetComponentInChildren<Melee>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        attackCollider.SetActive(isCharging);
        if (isCharging)
        {
            CheckPlayerInSector();
        }
    }

    
    void CheckAttack()
    {
        var playerInAttackRange = Physics2D.OverlapBox(
            transform.position + transform.up * mAI.attackDistance / 2,
            new Vector2(1.5f, mAI.attackDistance),
            mAI.rb.rotation,
            1 << LayerMask.NameToLayer("Player")
            );
        if (playerInAttackRange != null)
        {
            mAI.attack = true;
        }
        else
        {
            mAI.attack = false;
        }
        mAI.ani.SetBool("attack", mAI.attack);
    }

    void CheckPlayerInSector()
    {
        if (Mathf.Abs(Clusternoid.Math.RotationAngleFloat(
            transform.position, PlayerController.groupCenter.transform.position))
            > 180)
        {
            mAI.ani.SetTrigger("playerNotInRange");
        }
    }
}
