using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public Transform firingPosition;
    public float maxSpeed;

    //GameObject weapon;// 일단 무기를 여기다 담고 있는다. 
    //장기적으로는 weapon.cs로 옮겨서 enum으로 불러오는 것을 지원해야 함(그래야 다양한 종류의 무기를 지원가능)
    public float bulletSpeed; // 이것도 weapon.cs로 빼서 각 무기마다 속성이 달라야 한다.
    Vector2 destination;
    
    void Attack()
    {
        //원래는 weapon.cs에 명령을 내려서 쏘게 하는게 논리적으로 맞겠지만 (virtual 뭐시기를 썻던가)
        //일단 여기다 함수를 만들고 나중에 옮기기로 한다.
        var bullet = Instantiate(GameManager.instance.bullet, firingPosition.position, firingPosition.rotation);
//        bullet.GetComponent<Bullet>().Initialize();
        bullet.GetComponent<Rigidbody2D>().velocity = (bullet.transform.forward * bulletSpeed);
        Destroy(bullet, 2.0f); // 임시로 2초뒤에 파괴. 정식에는 필요 없을(수도 있음).
    }

    void Update()
    {
        float step = maxSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
        transform.rotation = GameManager.instance.player.transform.rotation;
        
    }

    void MoveTo(Vector2 pos)
    {
        destination = pos;
    }
}
