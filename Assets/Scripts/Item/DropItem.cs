using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class DropItem : MonoBehaviour {

    public List<GameObject> list;

    public void Drop()
    {
        foreach (var item in list)
        {
            Instantiate(item, Math.RandomOffsetPosition(transform.position, 0.1f), transform.rotation);
        }
    }
}
