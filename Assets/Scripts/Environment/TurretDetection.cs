using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{

    public List<Turret> turrets;

    Collider2D col;
    ContactFilter2D filter;
    Collider2D[] results;
    List<Character> characters;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        filter = new ContactFilter2D()
        {
            useTriggers = true
        };
        filter.SetLayerMask(1 << LayerMask.NameToLayer("Player"));
        results = new Collider2D[32];
        if (turrets.Count == 0)
        {
            Debug.LogError("터렛이 할당되지 않았습니다");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) => SendInfo(collision);
    private void OnTriggerExit2D(Collider2D collision) => SendInfo(collision);
    private void OnTriggerStay2D(Collider2D collision) => SendInfo(collision);
    

    void SendInfo(Collider2D collision)
    {
        col.OverlapCollider(filter, results);
        var isDetected = results[0] != null;
        foreach (var turret in turrets)
        {
            turret.isTargetInRange = isDetected;
        }

    }


}