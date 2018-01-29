using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class Shooters : Singleton<Shooters>
{

    public BulletShooter[] list;


    private void Start()
    {
        list = new BulletShooter[32];
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            list[child.layer] = child.GetComponent<BulletShooter>();
        }
    }

    /// <summary>
    /// layer에 따라 알맞은 Shooter를 반환해주는 함수이다.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public BulletShooter GetShooter(int layer)
    {
        return list[layer];
    }
}

