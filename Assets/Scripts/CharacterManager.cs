using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;


/// <summary>
/// 플레이어가 조작하는 다수의 캐릭터들 하나 하나가 들고 있는 스크립트. 
/// </summary>
/// 
public class CharacterManager : MonoBehaviour
{
    public bool isInsider;
    public bool alive;
    public float maximumDistance = 20f;
    public float speed = 6f; // The speed that the player will move at.
    public float repulsionCollisionRadius; // Repulse all characters that are in this radius
    public float repulsionIntensity; // Intensity of repulsion.


    //GameObject weapon;// 일단 무기를 여기다 담고 있는다. 
    //장기적으로는 weapon.cs로 옮겨서 enum으로 불러오는 것을 지원해야 함(그래야 다양한 종류의 무기를 지원가능)

    Vector3 movement; // The vector to store the direction of the player's movement.
    Rigidbody2D rb;
    Animator ani;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponentInChildren<Animator>();
        isInsider = true;
        alive = true;
        repulsionCollisionRadius = 1.5f;
        repulsionIntensity = 100f;
    }

    void Attack()
    {
        ani.SetTrigger("Attack");
        //gameObject.GetComponent<Weapon>().SendMessage("TryToFire");
    }


    void Update()
    {
        ani.SetBool("isInsider", isInsider);

        ///  float step = maxSpeed * Time.deltaTime;
        //  transform.position = Vector3.MoveTowards(transform.position, destination, step);
        //  transform.rotation = GameManager.instance.player.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            if (isInsider)
            {
                // Store the input axes.
                int h = (int) Input.GetAxisRaw("Horizontal");
                int v = (int) Input.GetAxisRaw("Vertical");
                // TODO: 키보드 입력의 경우 int처럼 빠릿빠릿해야 하고(-1 -> 0 -> 1) 컨트롤러 입력의 경우 적절한 입력 곡선을 가져야 함

                // Move the player around the scene.
                Move(h, v);

                // TODO: 만약 insider면 Move로 움직이고, 아니면 Playercontroller.groupCenter로 움직인다.

                //if (Vector2.Distance(rb.position, destination) > 1f)
                //{
                //    var direction = Vector2.zero;
                //    direction = destination - rb.position;
                //    rb.MovePosition(rb.position + direction.normalized * Time.deltaTime * maxSpeed);
                //} else
                //{
                //    rb.velocity = Vector2.zero;
                //}
                rb.rotation = PlayerController.groupCenter.GetComponent<Rigidbody2D>().rotation;
                //rb.MoveRotation(PlayerController.groupCenter.GetComponent<Rigidbody2D>().rotation);
            }
            else if (Vector2.Distance(
                         PlayerController.groupCenter.GetComponent<PlayerController>().centerOfGravityCharacter
                             .GetComponent<Rigidbody2D>().position, rb.position) < maximumDistance)
            {
                var direction = Vector2.zero;
                direction = PathFinder.GetAcceleration(transform.position);
                rb.MovePosition(rb.position + direction.normalized * Time.deltaTime * speed);
            }
        }
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, v, 0f);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;
        ani.SetFloat("velocity", movement.magnitude * 10);

        // Move the player to it's current position plus the movement.
        rb.MovePosition(transform.position + movement);
        //transform.Translate(movement, Space.World);
    }

    public bool IsRepulsing(CharacterManager otherCharacter)
    {
        // Check if this character is repulsing the other character
        var distanceVector = otherCharacter.transform.position - transform.position;
        return distanceVector.sqrMagnitude < repulsionCollisionRadius * repulsionCollisionRadius;
    }

    public void AddForce(Vector3 force, ForceMode2D mode = ForceMode2D.Force)
    {
        // Add force to this character's rigidbody. Exists for capsulation's sake.
        rb.AddForce(force, mode);
    }

    void KillCharacter()
    {
        //anim(투명하게 만들기)
        //remove from characters
        //Destroy
        isInsider = false;
        alive = false; //고기방패 상태
        ani.SetTrigger("isHit");
    }
}