using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject bullet;
    public GameObject healthPack; 
    // TODO: ObjectPool.cs를 구현해서 거기에서 들고 있고(BulletPool.cs? 등?)
    // Instantiate() 하던 부분을 모두 Get()을 활용하도록 바꾸기

    public static Quaternion RotationAngle(Vector2 from, Vector2 to)
    {
        Vector2 from2to = to - from;
        from2to.Normalize();
        float rot_z = Mathf.Atan2(from2to.y, from2to.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
