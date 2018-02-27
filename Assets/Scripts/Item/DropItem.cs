using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class DropItem : MonoBehaviour {

    float dropChecker;

    public float dropPercent;

    public List<GameObject> list;

    public void Drop()
    {
        foreach (var item in list)
        {
            dropChecker = Random.value;

            if(dropPercent == 0)
            {
                return;
            }
            else if(Random.value <= dropPercent/100)
            {
                Instantiate(item, Math.RandomOffsetPosition(transform.position, 0.1f), transform.rotation);
            }
        }
    }
}
