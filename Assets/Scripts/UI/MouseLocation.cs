using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocation : MonoBehaviour {

    Plane xyPlane;
    float confineSize;
	// Use this for initialization
	void Start () {
        xyPlane = new Plane(Vector3.forward, Vector3.zero);
        confineSize = Mathf.Min(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update () {

        var pos = Input.mousePosition;
        
        pos.x = Mathf.Clamp(pos.x, confineSize * 0.1f, confineSize * 0.9f);
        pos.y = Mathf.Clamp(pos.y, confineSize * 0.1f, confineSize * 0.9f);
        var camRay = Camera.main.ScreenPointToRay(pos);
        float distance;
        if (xyPlane.Raycast(camRay, out distance))
        {
            transform.position = camRay.GetPoint(distance);
        }
    }
}
