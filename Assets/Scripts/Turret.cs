using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI))]
public class Turret : MonoBehaviour {

    Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //GetComponent<Weapon>().SendMessage("TryToFire");
        ani.SetBool("fire", ani.GetBool("targetFound"));
	}


}
