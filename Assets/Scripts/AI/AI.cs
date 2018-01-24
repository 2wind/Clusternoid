using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour {
    
    Animator ani;


    private void Start()
    {
        ani = GetComponent<Animator>();

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
