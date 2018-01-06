using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firingPosition;
    public int damage = 3;
    public float bulletSpeed = 1000; // 이것도 weapon.cs로 빼서 각 무기마다 속성이 달라야 한다.
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
        //원래는 weapon.cs에 명령을 내려서 쏘게 하는게 논리적으로 맞겠지만 (virtual 뭐시기를 썻던가)
        //일단 여기다 함수를 만들고 나중에 옮기기로 한다.
        //TODO: 부하 감소를 위해 IEnumerator으로 구현하기
        var bullet = Instantiate(GameManager.instance.bullet, firingPosition.position, firingPosition.rotation);
        bullet.GetComponent<Bullet>().Initialize(gameObject.tag);
        firingPosition.gameObject.GetComponent<AudioSource>().Play();
        Destroy(bullet.gameObject, 2.0f); // 임시로 2초뒤에 파괴. 정식에는 필요 없을(수도 있음).
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
    }
}
