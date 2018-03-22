using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{

    public GameObject laserBeamPrefab;
    [Range(0, 10)] public float radius = 1;
    public RaycastHit2D[] hits;
    private bool isActive;
    public float duration = 1.5f;


    public override void Fire()
    {
        isActive = true;
        //firingPosition.gameObject.GetComponent<SoundPlayer>().Play();
        Invoke(nameof(StopFiring), duration);
    }

    private void StopFiring()
    {
        isActive = false;
    }

    public void Update()
    {
        if (isActive)
        {
            var check = Physics2D.Raycast(firingPosition.transform.position, transform.up, 30);
            float distance = 30;
            if (check.collider != null)
            {
                distance = check.distance;
            }
            hits = Physics2D.CircleCastAll(
                firingPosition.transform.position,
                radius,
                transform.up,
                distance);
            foreach (var hit in hits)
            {
                var attack = new Attack(tag.GetHashCode(), damage, firingPosition.rotation.eulerAngles, 0, 0);
                var hl = hit.transform.GetComponent<HitListener>();
                hl?.TriggerListener(attack);
            }
        }
    }

}
