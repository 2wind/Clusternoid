using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform firingPosition;
    public int damage = 3;
    public float bulletSpeed = 100; // 이것도 weapon.cs로 빼서 각 무기마다 속성이 달라야 한다.
    public float spread = 0.1f; //무기의 탄 퍼짐 가능성. 0이면 언제나 정확함. = 확률분포의 sigma값.

    public float preShoot = 100f; //무기의 선딜 애니메이션의 재생 속도 배수. >99이면 선딜이 재생되지 않는다.

    public bool isEmittingSound = true;
    protected bool IsPlayer() => gameObject.CompareTag("Player");

    private void Start()
    {
        if (gameObject.CompareTag("Player"))
            GetComponentInChildren<Animator>().SetFloat("pre-attack", preShoot);
        if (firingPosition == null)
        {
            firingPosition = transform.Find("Firing Position");
        }
    }

    public abstract void Fire();
}