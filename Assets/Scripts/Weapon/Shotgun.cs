using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

    public int angle = 15;

    public override void Fire()
    {
        if (gameObject.CompareTag("Player"))
        {
            firingPosition.gameObject.GetComponent<SoundPlayer>().Play(SoundType.Weapon_Shotgun_Fire);
        }
        var spreadAngle = Clusternoid.Math.NextGaussian(0, spread, -45, 45);
        for (int i = 0; i < 3; i++)
        {
            var bullet = BulletPool.Get("bullet");
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = firingPosition.rotation;
            bullet.transform.Rotate(new Vector3(0, 0, angle * (i - 1) + spreadAngle));
            bullet.GetComponent<Bullet>().Initialize(gameObject.tag, bulletSpeed, damage);
        }
    }


}

