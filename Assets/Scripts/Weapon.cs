using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clusternoid;

public class Weapon : MonoBehaviour
{

    public Transform firingPosition;
    public int damage = 3;
    public float bulletSpeed = 100; // 이것도 weapon.cs로 빼서 각 무기마다 속성이 달라야 한다.
    public float cooldown = 1f; //무기의 쿨다운
    public float spread; //무기의 탄 퍼짐 가능성. 0이면 언제나 정확함. 
    float timer;

    private void Awake()
    {
        timer = cooldown;
    }

    void TryToFire()
    {
        if (timer < 0)
        {
            Fire();
            timer = cooldown;
        }
    }

    void Fire()
    {
        //TODO: 부하 감소를 위해 IEnumerator으로 구현하기
        //TODO: 무기 종류&탄약 종류에 따라 다양한 총알 발사하기
        //var bullet = BulletPool.Get("bullet");
        //bullet.transform.position = firingPosition.position;
        //bullet.transform.rotation = firingPosition.rotation;
        //bullet.GetComponent<Bullet>().Initialize(gameObject.tag, bulletSpeed);
        firingPosition.gameObject.GetComponent<AudioSource>().Play();

        Shooters.instance.GetShooter(gameObject.layer).Shoot(firingPosition.position, firingPosition.up);
        
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
    }
}
