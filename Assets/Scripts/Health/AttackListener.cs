using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

public struct Attack
{
    public readonly int attackerTag;
    public readonly Guid id;
    public readonly int damage;
    public readonly Vector3 direction;
    public readonly float knockback;
    public readonly float refreshTime;

    public Attack(int tag, int damage, Vector3 direction, float knockback, float refresh)
    {
        attackerTag = tag;
        this.damage = damage;
        this.direction = direction;
        this.knockback = knockback;
        refreshTime = refresh;
        id = Guid.NewGuid();
    }

    public override string ToString()
    {
        var s = $"AttackerTag: {attackerTag}, ID: {id} Damage: {damage}, Direction: {direction}, Knockback: {knockback}, RefreshTime: {refreshTime}";
        return s;
    }
}

[UsedImplicitly]
[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class AttackData
{
    public int damage;
    public float knockback;
    public bool repeat;
    public float refreshTime;
}

public class AttackListener : MonoBehaviour
{
    int tagHash;
    public AttackData attackData;
    public Action onAttackSuccess;

    void Start()
    {
        tagHash = gameObject.tag.GetHashCode();
    }

    void OnTriggerEnter2D(Collider2D other) => TriggerCaller(other);
    void OnTriggerStay2D(Collider2D other) => TriggerCaller(other);

    void TriggerCaller(Collider2D other)
    {
        var diff = gameObject.transform.position - other.transform.position;

        var success = other.GetComponent<HitListener>()?.TriggerListener(new Attack(tagHash,
            attackData.damage,
            diff,
            attackData.knockback,
            attackData.repeat ? attackData.refreshTime : float.PositiveInfinity));
        if (success.HasValue && success.Value) onAttackSuccess?.Invoke();
    }
}