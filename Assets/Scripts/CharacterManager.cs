using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {



    /// <summary>
    /// 플레이어가 조작하는 다수의 캐릭터들 하나 하나가 들고 있는 스크립느. 
    /// </summary>
    /// 
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
        gameObject.GetComponent<Weapon>().SendMessage("TryToFire");
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
