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
        if (PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestDistance(transform.position)
            < alertDistance)
        {
        ani.SetBool("targetInRange", true);
            //TODO: 조금 더 효율적인 방법으로 바꾸기
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
