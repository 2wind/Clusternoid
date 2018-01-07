using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class Shooters : Singleton<Shooters>
{

    public BulletShooter playerShooter;
    public BulletShooter enemyShooter;

    /// <summary>
    /// layer에 따라 알맞은 Shooter를 반환해주는 함수이다.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public BulletShooter GetShooter(int layer)
    {
        switch (layer)
        {
            case 9:
                return playerShooter;
            case 10:
                return enemyShooter;
            default:
                Debug.Log("layer number incorrect: number " + layer);
                return enemyShooter;
        }
    }
}

