using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocation : MonoBehaviour {

    Plane xyPlane;
    float confineSize;
    Vector3 toCenter;
	// Use this for initialization
	void Start () {
        xyPlane = new Plane(Vector3.forward, Vector3.zero);
        confineSize = Mathf.Min(Screen.width, Screen.height);
        toCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update () {

        var pos = Input.mousePosition;
        var fromCenter = pos - toCenter;
        fromCenter = Vector3.ClampMagnitude(fromCenter, confineSize * 0.9f);
        //pos.x = Mathf.Clamp(pos.x, confineSize * 0.1f, confineSize * 0.9f);
        //pos.y = Mathf.Clamp(pos.y, confineSize * 0.1f, confineSize * 0.9f);
        var newPos = fromCenter + toCenter;
        var camRay = Camera.main.ScreenPointToRay(newPos);
        float distance;
        if (xyPlane.Raycast(camRay, out distance))
        {
            transform.position = camRay.GetPoint(distance);
        }
    }
}
