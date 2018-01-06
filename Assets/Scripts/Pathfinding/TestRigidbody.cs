using UnityEngine;

public class TestRigidbody : MonoBehaviour
{
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = PathFinder.GetAcceleration(transform.position) * speed;
    }
}