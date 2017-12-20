using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public Transform firingPosition;
    public float maxSpeed;

    //GameObject weapon;// 일단 무기를 여기다 담고 있는다. 
    //장기적으로는 weapon.cs로 옮겨서 enum으로 불러오는 것을 지원해야 함(그래야 다양한 종류의 무기를 지원가능)
    Vector2 destination;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Attack()
    {
        //원래는 weapon.cs에 명령을 내려서 쏘게 하는게 논리적으로 맞겠지만 (virtual 뭐시기를 썻던가)
        //일단 여기다 함수를 만들고 나중에 옮기기로 한다.
        var bullet = Instantiate(GameManager.instance.bullet, firingPosition.position, firingPosition.rotation);
        bullet.GetComponent<Bullet>().Initialize();
        
        Destroy(bullet.gameObject, 2.0f); // 임시로 2초뒤에 파괴. 정식에는 필요 없을(수도 있음).
    }

    void Update()
    {
        ///  float step = maxSpeed * Time.deltaTime;
        //  transform.position = Vector3.MoveTowards(transform.position, destination, step);
        //  transform.rotation = GameManager.instance.player.transform.rotation;

    }

    private void FixedUpdate()
    {

        if (Vector2.Distance(rb.position, destination) > 1f)
        {
            var direction = Vector2.zero;
            direction = destination - rb.position;
            rb.MovePosition(rb.position + direction.normalized * Time.deltaTime * maxSpeed);
        } else
        {
            rb.velocity = Vector2.zero;
        }
        rb.MoveRotation(GameManager.instance.player.GetComponent<Rigidbody2D>().rotation);


    }

    void MoveTo(Vector2 pos)
    {
        destination = pos;
    }
}
