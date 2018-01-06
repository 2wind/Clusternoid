using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Weapon>().SendMessage("TryToFire");
	}

    private void FixedUpdate()
    {
        transform.rotation = Clusternoid.Math.RotationAngle(this.transform.position, PlayerController.player.transform.position);
        //TODO: 이 친구들이 제대로 돌도록 함수를 넣어 주어야 한다.
    }
}
