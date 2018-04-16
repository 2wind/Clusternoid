using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public int angle = 15;

    public override void Fire()
    {
        var player = IsPlayer();
        if (player && isEmittingSound)
        {
            firingPosition.gameObject.GetComponent<SoundPlayer>().Play(SoundType.Weapon_Shotgun_Fire);
        }
        else if (!player)
        {
            firingPosition.gameObject.GetComponent<SoundPlayer>().Play(SoundType.Enemy_ShotgunRobot_Fire);
        }
        var spreadAngle = Clusternoid.Math.NextGaussian(0, spread, -45, 45);
        for (int i = 0; i < 3; i++)
        {
            var bullet = player ? BulletPool.Get("bullet") : BulletPool.Get("circlebullet");
            bullet.Transform.position = firingPosition.position;
            bullet.Transform.rotation = firingPosition.rotation;
            bullet.Transform.Rotate(new Vector3(0, 0, angle * (i - 1) + spreadAngle));
            bullet.Initialize(gameObject.tag, bulletSpeed, damage);
        }
    }
}