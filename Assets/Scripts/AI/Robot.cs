using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI))]
public class Robot : MonoBehaviour {

    Animator ani;
    Weapon wb;
    AI ai;
    Rigidbody2D rb;
    float dangerDistance;

    // Use this for initialization
    void Start () {
        ani = GetComponent<Animator>();
        wb = GetComponent<Weapon>();
        ai = GetComponent<AI>();
        rb = GetComponent<Rigidbody2D>();
        dangerDistance = ai.alertDistance * 0.3f;
    }

    private void Update()
    {
        var distance = PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestDistance(transform.position);
        if (distance < dangerDistance)
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

    private void FixedUpdate()
    {
        
    }

}
