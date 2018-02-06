using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DPS Checker for player character
/// </summary>
public class DPSChecker : MonoBehaviour {

    Animator ani;
    AnimatorStateInfo prestate, state;
    float time = 0f;
    int fired = 0;

	// Use this for initialization
	void Start () {
        ani = GetComponentInChildren<Animator>();
        prestate = ani.GetCurrentAnimatorStateInfo(1);
        state = ani.GetCurrentAnimatorStateInfo(1);
	}
	
	// Update is called once per frame
	void Update () {
        prestate = state;
        state = ani.GetCurrentAnimatorStateInfo(1);
        if (Input.GetButton("Fire1"))
        {
            time += Time.deltaTime;
            
            if (!prestate.IsName("shoot") && state.IsName("shoot"))
            {
                fired += 1;
            }
            Debug.Log("DPS: " + fired / (time));
        }

    }
}
