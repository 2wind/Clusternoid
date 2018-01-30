using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI))]
public class Robot : MonoBehaviour {

    Animator ani;
    Weapon wb;
    AI ai;
    Rigidbody2D rb;
    RaycastHit2D hit;


    public float alertDistance = 8f;
    public float attackDistance = 5f;
    float dangerDistance;

    bool targetInRange;
    bool attack;
    public bool superArmor;
    public Vector2 path;

    // 플레이어를 인식하는 +- 각도 in degree
    public float degree = 45; 
    private float degreeRadCosine;


    // Use this for initialization
    void Start () {
        ani = GetComponentInChildren<Animator>();
        wb = GetComponent<Weapon>();
        ai = GetComponent<AI>();
        rb = GetComponent<Rigidbody2D>();
        dangerDistance = attackDistance * 0.3f;

        targetInRange = false;
        attack = false;
        superArmor = false;
        path = Vector2.zero;
        degreeRadCosine = Mathf.Cos(degree * Mathf.Deg2Rad);
    }


    private void Update()
    {
        CheckAlert();
        CheckAttack();
        CheckDanger();
        CheckObstacle();
        
        ani.SetBool("hitResist", superArmor);


    }

    void CheckObstacle()
    {
        ai.FindNearestCharacter();
        var marginVector = (ai.nearestCharacter.transform.position - transform.position).normalized;
        hit = Physics2D.Linecast(transform.position + marginVector,
            ai.nearestCharacter.transform.position);


        if (!hit.collider.CompareTag("Player"))
        {
            ani.SetBool("obstacle", true);
        }
        else
        {
            ani.SetBool("obstacle", false);
        }
    }

    void CheckAlert()
    {
        var playerInAlertRange = Physics2D.OverlapCircle(transform.position, alertDistance, 1 << LayerMask.NameToLayer("Player"));
        if (playerInAlertRange != null)
        {
            targetInRange = true;
        }
        else
        {
            targetInRange = false;
        }
        ani.SetBool("targetInRange", targetInRange);
    }

    void CheckAttack()
    {
        var playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackDistance, 1 << LayerMask.NameToLayer("Player"));

        if (playerInAttackRange != null)
        {
            var dotProduct = Vector2.Dot(transform.up, transform.InverseTransformPoint(playerInAttackRange.transform.position).normalized);
            if (dotProduct > degreeRadCosine)
            {
                Debug.Log(dotProduct);
                attack = true;
            }
        }
        else
        {
            attack = false;
        }
        ani.SetBool("attack", attack);

    }

    void CheckDanger()
    {
        if (Physics2D.OverlapCircle(transform.position, dangerDistance, 1 << LayerMask.NameToLayer("Player")) != null)
        {
            ani.SetBool("moveBack", true);
            RaycastHit2D hit = Physics2D.Raycast(
            wb.firingPosition.position,
            transform.up,
            1f
            );
            if (hit.collider != null)
            {
                ani.SetBool("moveBack", false);
                ani.SetTrigger("attack");
            }
        }
        else
        {
            ani.SetBool("moveBack", false);
        }
    }

    



}
