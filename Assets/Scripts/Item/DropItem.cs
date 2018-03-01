using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class DropItem : MonoBehaviour {

    public float dropPercent;

    public List<GameObject> list;

    public void Drop()
    {
        foreach (var item in list)
        {
            if(dropPercent < Mathf.Epsilon)
            {
                return;
            }
            else if(Random.value < dropPercent/100 + Mathf.Epsilon)
            {
                Instantiate(item, Math.RandomOffsetPosition(transform.position, 0.1f), transform.rotation);
            }
        }
    }
}
