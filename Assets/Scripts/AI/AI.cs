using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (Physics2D.OverlapCircle(transform.position, alertDistance, 1 << LayerMask.NameToLayer("Player")) != null)
        {
            ani.SetBool("targetInRange", true);
        }
        else
        {
            ani.SetBool("targetInRange", false);
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
