using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {



    /// <summary>
    /// 플레이어가 조작하는 다수의 캐릭터들 하나 하나가 들고 있는 스크립트. 
    /// </summary>
    /// 

    public bool isInsider;
    public float speed = 6f;            // The speed that the player will move at.


    //GameObject weapon;// 일단 무기를 여기다 담고 있는다. 
    //장기적으로는 weapon.cs로 옮겨서 enum으로 불러오는 것을 지원해야 함(그래야 다양한 종류의 무기를 지원가능)

    Vector3 movement;                   // The vector to store the direction of the player's movement.
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

        // Store the input axes.
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");
        // TODO: 키보드 입력의 경우 int처럼 빠릿빠릿해야 하고(-1 -> 0 -> 1) 컨트롤러 입력의 경우 적절한 입력 곡선을 가져야 함

        // Move the player around the scene.
        Move(h, v);

        //if (Vector2.Distance(rb.position, destination) > 1f)
        //{
        //    var direction = Vector2.zero;
        //    direction = destination - rb.position;
        //    rb.MovePosition(rb.position + direction.normalized * Time.deltaTime * maxSpeed);
        //} else
        //{
        //    rb.velocity = Vector2.zero;
        //}
        rb.MoveRotation(PlayerController.player.GetComponent<Rigidbody2D>().rotation);


    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, v, 0f);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        transform.Translate(movement, Space.World);
    }

}
