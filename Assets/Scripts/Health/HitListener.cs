using System;
using System.Collections.Generic;
using UnityEngine;

public class HitListener : MonoBehaviour
{
    readonly Dictionary<Guid, float> timer = new Dictionary<Guid, float>();
    Health myHealth;

    void Start()
    {
        myHealth = GetComponentInParent<Health>();
    }

    public bool TriggerListener(Attack attack)
    {
        if (timer.ContainsKey(attack.id))
        {
            if (timer[attack.id] > Time.time) return false;
            timer[attack.id] = Time.time + attack.refreshTime;
        }
        else
        {
            timer.Add(attack.id, Time.time + attack.refreshTime);
        }
        return myHealth.GetAttack(attack);
    }
}