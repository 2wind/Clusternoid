using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public Transform firingPosition;
    public int damage = 3;
    public float bulletSpeed = 100; // 이것도 weapon.cs로 빼서 각 무기마다 속성이 달라야 한다.
    public float spread = 0.1f; //무기의 탄 퍼짐 가능성. 0이면 언제나 정확함. = 확률분포의 sigma값.


    public abstract void Fire();

}
