using System;
using UnityEngine;


/// <inheritdoc />
/// <summary>
/// 플레이어가 조작하는 다수의 캐릭터들 하나 하나가 들고 있는 스크립트. 
/// </summary>
public class Character : MonoBehaviour
{
    public bool isInsider;
    public bool alive;
    public float speed = 6f; // The speed that the player will move at.
    public float repulsionCollisionRadius; // Repulse all characters that are in this radius
    public float repulsionIntensity; // Intensity of repulsion.

    [NonSerialized] public Vector2 repulsion;

    Vector3 movement; // The vector to store the direction of the player's movement.
    Rigidbody2D rb;
    Animator ani;
    float rotateSpd;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponentInChildren<Animator>();
        isInsider = true;
        alive = true;
    }

    void Update()
    {
        ani.SetBool("isInsider", isInsider);
        if (alive && isInsider)
        {
            if (Input.GetButton("Fire1"))
                ani.SetTrigger("Attack");
        }

    }

    void FixedUpdate()
    {
        if (!alive) return;
        var direction = PathFinder.GetAcceleration(transform.position);
        if (isInsider)
        {
            if (Vector2.Distance(PathFinder.instance.target.position, transform.position) < 0.5f)
                direction = Vector2.zero;
            // Move the player around the scene.
            Move(PlayerController.groupCenter.input * 0.5f + direction + repulsion);
            rb.rotation = PlayerController.groupCenter.GetComponent<Rigidbody2D>().rotation;
        }
        else
        {
            rb.MovePosition(rb.position + direction.normalized * Time.deltaTime * speed);
            var targetRot = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            rb.rotation = Mathf.SmoothDampAngle(rb.rotation, targetRot, ref rotateSpd, 0.5f);
        }
        repulsion = Vector2.zero;
    }

    void Move(Vector2 direction)
    {
        if (direction.magnitude > 1f) direction.Normalize();
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = direction * speed * Time.deltaTime;
        ani.SetFloat("velocity", movement.magnitude * 10);
        if (direction.magnitude < 0.5f) return;

        // Move the player to it's current position plus the movement.
        rb.MovePosition(transform.position + movement);
        //transform.Translate(movement, Space.World);
    }

    public bool IsRepulsing(Character otherCharacter) =>
        Vector2.Distance(otherCharacter.transform.position, transform.position) < repulsionCollisionRadius;

    void KillCharacter()
    {
        //remove from characters
        isInsider = false;
        alive = false; //고기방패 상태
        ani.SetTrigger("isHit");
    }
}