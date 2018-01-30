using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

    public override void Fire()
    {
        firingPosition.gameObject.GetComponent<AudioSource>().Play();
        var spreadAngle = Clusternoid.Math.NextGaussian(0, spread, -45, 45);
        var angle = 10;
        for (int i = 0; i < 2; i++)
        {
            var bullet = BulletPool.Get("bullet");
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = firingPosition.rotation;
            bullet.transform.Rotate(new Vector3(0, 0, angle * (i - 1) + spreadAngle));
            bullet.GetComponent<Bullet>().Initialize(gameObject.tag, bulletSpeed, damage);
        }
    }


}
