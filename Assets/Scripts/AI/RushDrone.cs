using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI))]
public class RushDrone : MonoBehaviour {

    Animator ani;
    Weapon wb;
    AI ai;
    Rigidbody2D rb;

    public bool isCharging;
    // Use this for initialization
    void Start () {
        ani = GetComponentInChildren<Animator>();
        wb = GetComponent<Weapon>();
        ai = GetComponent<AI>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if (isCharging)
        {

        }
	}
}
