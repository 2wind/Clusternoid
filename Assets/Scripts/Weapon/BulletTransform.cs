using UnityEngine;

public class BulletTransform
{
    public Vector3 velocity;
    public Vector3 up => rotation * Vector3.up;
    public Vector3 position;
    public Quaternion rotation;
    public Quaternion drawRotation => rotation * Quaternion.Euler(0, 0, 90);

    public void Rotate(Vector3 vector3) => rotation *= Quaternion.Euler(vector3);
    public void Update() => position += velocity * Time.fixedDeltaTime;
}