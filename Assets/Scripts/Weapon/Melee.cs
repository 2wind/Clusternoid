using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {

    Collider2D col;
    public int damage;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public void SetActive(bool setting)
    {
        col.enabled = setting;
    }

    // OnTriggerEnter2D는 Collider2D other가 트리거가 될 때 호출됩니다(2D 물리학에만 해당).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var attack = new Attack(gameObject.tag.GetHashCode(), damage, transform.up, 2, 0);
        var hl = collision.GetComponent<HitListener>();
        hl?.TriggerListener(attack);
    }



}
