using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraWait : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(WaitAndGiveControl(1.5f));
    }

    IEnumerator WaitAndGiveControl(float duration)
    {
        yield return new WaitForSeconds(duration);
        _virtualCamera.Priority = 5;
    }
}
