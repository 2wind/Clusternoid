using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class TestShooter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			GetComponent<IShooter>()?.Shoot();
		}
	}
}
