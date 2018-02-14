using System;
using System.Collections;
using System.Collections.Generic;
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
    public float evadeDuration = 1.0f;
    public float evadeCooldown = 2.0f;
    float evadeTime = 0.0f;
    float cooldown = 0.0f;

    [NonSerialized] public Vector2 repulsion;

    Vector2 movement; // The vector to store the direction of the player's movement.
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
         //   if (Vector2.Distance(PathFinder.instance.target.position, transform.position) < 0.5f)
          //      direction = Vector2.zero;
            // Move the player around the scene.
            var distanceToTarget = Vector2.Distance(PathFinder.instance.target.position, transform.position);
            if (distanceToTarget < 1f)
            {
                direction = direction * distanceToTarget;
            }
            Move(PlayerController.groupCenter.input + direction);

            //Move(PlayerController.groupCenter.input * 0.5f + direction);
            //Move(PlayerController.groupCenter.input); /// 1. 입력에 의한 이동만 실시한다. 뭔가 이상함.
            rb.AddForce(repulsion * speed);
            rb.rotation = PlayerController.groupCenter.GetComponent<Rigidbody2D>().rotation;
            Evade(PlayerController.groupCenter.input);

        }
        else
        {
            rb.AddForce(direction * speed * rb.mass);
            //rb.MovePosition(rb.position + direction.normalized * Time.fixedDeltaTime * speed);
            var targetRot = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            rb.rotation = Mathf.SmoothDampAngle(rb.rotation, targetRot, ref rotateSpd, 0.5f);
        }
        repulsion = Vector2.zero;

        //회피 기동
    }

    void Move(Vector2 direction)
    {
        direction = Vector2.ClampMagnitude(direction, 1);
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = direction * speed * Time.fixedDeltaTime;
        ani.SetFloat("velocity", movement.magnitude * 10);
        if (direction.magnitude < 0.5f) return;

        // Move the player to it's current position plus the movement.
        //rb.AddForce(direction * speed / Time.fixedDeltaTime);
        //var vel = rb.velocity;
        //Vector2.SmoothDamp(rb.position, rb.position + movement, ref vel, 1/speed , Mathf.Infinity, Time.fixedDeltaTime);
        //rb.AddForce(direction * speed * rb.mass);
        //Vector2.MoveTowards(rb.position, rb.position + movement, Time.fixedDeltaTime);
        rb.velocity = direction * speed;
        //rb.MovePosition(rb.position + movement);
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

    void Evade(Vector2 evadeDirection)
    {
        if (cooldown > 0.0f)
        {
            if (evadeTime > 0.0f)
            {
                evadeTime -= Time.fixedDeltaTime;
                rb.MovePosition(rb.position + evadeDirection.normalized * 20 * Time.fixedDeltaTime);
                //  Debug.Log(evadeTime);
            }
            cooldown -= Time.fixedDeltaTime;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            evadeTime = evadeDuration;
            cooldown = evadeCooldown;
            ani.SetTrigger("Evade");
           // Debug.Log(evadeDirection);
        }
    }
}