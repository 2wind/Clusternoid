using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + Vector3.back, "Triggers/start.png", true);
    }
}
