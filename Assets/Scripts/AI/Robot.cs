using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovingAI))]
public class Robot : MonoBehaviour
{
    float dangerDistance;

    // 플레이어를 인식하는 +- 각도 in degree
    public float degree = 45;

    private float degreeRadCosine;

    [HideInInspector] public MovingAI mAI;


    // Use this for initialization
    void Start()
    {
        mAI = GetComponent<MovingAI>();
        dangerDistance = mAI.attackDistance * 0.3f;
        degreeRadCosine = Mathf.Cos(degree * Mathf.Deg2Rad);
    }


    void Update()
    {
        if (PlayerController.groupCenter.characters.Any())
        {
            CheckAttack();
            //CheckDanger();

            mAI.ani.SetBool("hitResist", mAI.superArmor);
        }
    }


    void CheckAttack()
    {
        var playerInAttackRange =
            Physics2D.OverlapCircle(transform.position, mAI.attackDistance, 1 << LayerMask.NameToLayer("Player"));

        if (playerInAttackRange != null)
        {
            var dotProduct = Vector2.Dot(transform.up,
                (playerInAttackRange.transform.position - transform.position).normalized);
            mAI.attack = dotProduct > degreeRadCosine;
        }
        else
        {
            mAI.attack = false;
        }
        mAI.ani.SetBool("attack", mAI.attack);
    }

    void CheckDanger()
    {
        if (Physics2D.OverlapCircle(transform.position, dangerDistance, 1 << LayerMask.NameToLayer("Player")) != null)
        {
            mAI.ani.SetBool("moveBack", true);
            RaycastHit2D hit = Physics2D.Raycast(
                mAI.wb.firingPosition.position,
                transform.up,
                1f,
                LayerMask.GetMask("Wall", "IgnoreBullet", "Default")
            );
            if (hit.collider != null)
            {
                mAI.ani.SetBool("moveBack", false);
                mAI.ani.SetTrigger("attack");
            }
        }
        else
        {
            mAI.ani.SetBool("moveBack", false);
        }
    }
}