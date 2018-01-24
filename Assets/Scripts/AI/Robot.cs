using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI))]
public class Robot : MonoBehaviour {

    Animator ani;
    Weapon wb;
    AI ai;
    Rigidbody2D rb;

    public float alertDistance = 8f;
    public float attackDistance = 5f;
    float dangerDistance;

    // Use this for initialization
    void Start () {
        ani = GetComponent<Animator>();
        wb = GetComponent<Weapon>();
        ai = GetComponent<AI>();
        rb = GetComponent<Rigidbody2D>();
        dangerDistance = attackDistance * 0.3f;

    }

    
    private void Update()
    {
        CheckAlert();
        CheckAttack();
        CheckDanger();
    }

    void CheckAlert()
    {
        var playerInAlertRange = Physics2D.OverlapCircle(transform.position, alertDistance, 1 << LayerMask.NameToLayer("Player"));
        if (playerInAlertRange != null)
        {
            ani.SetBool("targetInRange", true);
        }
        else
        {
            ani.SetBool("targetInRange", false);
        }
    }

    void CheckAttack()
    {
        var playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackDistance, 1 << LayerMask.NameToLayer("Player"));

        if (playerInAttackRange != null)
        {
            ani.SetBool("attack", true);
        }
        else
        {
            ani.SetBool("attack", false);
        }
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
