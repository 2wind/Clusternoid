using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    
    Animator ani;
    public float alertDistance;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
       if (PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestDistance(transform.position)
            < alertDistance)
        {
            ani.SetBool("targetFound", true);
        }
        else
        {
            ani.SetBool("targetFound", false);
        }

    }

    public void GetAttack()
    {
        ani.SetTrigger("hit");
    }

    public void SetDeath()
    {
        ani.SetTrigger("die");
    }


}
