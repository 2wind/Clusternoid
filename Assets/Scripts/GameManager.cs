using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject bullet;
    public GameObject healthPack;
    public GameObject player; //현재 플레이어. 나중에 코옵을 지원하게 되면 리스트가 되겠지?

    public static Quaternion RotationAngle(Vector2 from, Vector2 to)
    {
        Vector2 from2to = to - from;
        from2to.Normalize();
        float rot_z = Mathf.Atan2(from2to.y, from2to.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
