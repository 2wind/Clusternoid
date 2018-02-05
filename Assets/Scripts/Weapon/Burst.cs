using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : Weapon
{
    public int burst = 3;
    public float delay = 0.1f;

    public override void Fire() => StartCoroutine(BurstFire());


    IEnumerator BurstFire()
    {
        for (int i = 0; i < burst; i++)
        {
            firingPosition.gameObject.GetComponent<AudioSource>().Play();
            var spreadAngle = Clusternoid.Math.NextGaussian(0, spread, -45, 45);
            var bullet = BulletPool.Get("bullet");
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = firingPosition.rotation;
            bullet.transform.Rotate(new Vector3(0, 0, spreadAngle));
            bullet.GetComponent<Bullet>().Initialize(gameObject.tag, bulletSpeed, damage);
            yield return new WaitForSeconds(delay);
        }
    }
}
