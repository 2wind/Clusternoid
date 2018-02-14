using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocation : MonoBehaviour {

    Plane xyPlane;
	// Use this for initialization
	void Start () {
        xyPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    // Update is called once per frame
    void Update () {
        var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (xyPlane.Raycast(camRay, out distance))
        {
            transform.position = camRay.GetPoint(distance);
        }
    }
}
