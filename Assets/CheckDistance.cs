using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OverlapCircle 테스트용 스크립터.
/// </summary>
public class CheckDistance : MonoBehaviour {

    Collider2D coll;
    Vector3 movement;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
        coll = Physics2D.OverlapCircle(transform.position, 4f, 1 << LayerMask.NameToLayer("Player"));
        if (coll != null)
        {
            Debug.Log(coll.name);

        }
    }

    private void FixedUpdate()
    {
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");
        Move(h, v);

    }
    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, v, 0f);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * 10 * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        rb.MovePosition(transform.position + movement);
        //transform.Translate(movement, Space.World);
    }
}
