using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref https://unity3d.com/kr/learn/tutorials/projects/survival-shooter/camera-setup
public class CameraFollow : MonoBehaviour {

    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.

    void Start()
    {
        // Calculate the initial offset.
        //offset = transform.position - target.position;
        offset.x = 0;
        offset.y = transform.position.z * Mathf.Abs(Mathf.Tan(transform.rotation.eulerAngles.x));
        offset.z = transform.position.z;
        // TODO: 적절한 offset을 자동으로 찾을 것. 가능하면 확대 축소도.

    }

    void LateUpdate()
    {

        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = ((target.position) * 2  + GetMousePosition()) / 3;
        targetCamPos += offset;
        //targetCamPos.z = offset.z;
        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }


    public Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }
}
