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
        hit = Physics2D.Linecast(GetComponent<Weapon>().firingPosition.position,
            ai.nearestCharacter.transform.position);
        Debug.DrawLine(GetComponent<Weapon>().firingPosition.position,
            ai.nearestCharacter.transform.position);
        Debug.Log(hit.collider);
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
            attack = true;
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
