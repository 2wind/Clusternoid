using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    string GetPath(string target) => Path() + target;
    static string Path() => "Bullets/";
    readonly Dictionary<string, List<IBullet>> bulletPool = new Dictionary<string, List<IBullet>>();
    readonly Dictionary<string, Tuple<Mesh, Material>> rendered = new Dictionary<string, Tuple<Mesh, Material>>();
    readonly Dictionary<string, Tuple<Vector2, Vector2>> box = new Dictionary<string, Tuple<Vector2, Vector2>>();
    readonly Dictionary<string, List<Matrix4x4>> matrices = new Dictionary<string, List<Matrix4x4>>();
    readonly Collider2D[] collisions = new Collider2D[10];

    public static IBullet Get(string name) => instance.GetInstance(name);

    IBullet GetInstance(string bulletName)
    {
        if (!bulletPool.ContainsKey(bulletName))
        {
            var bullet = Resources.Load<GameObject>(GetPath(bulletName));
            var mat = bullet.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            var mesh = bullet.GetComponentInChildren<MeshFilter>().sharedMesh;
            var rect = bullet.GetComponent<BoxCollider2D>();
            rendered.Add(bulletName, new Tuple<Mesh, Material>(mesh, mat));
            matrices.Add(bulletName, new List<Matrix4x4>());
            box.Add(bulletName, new Tuple<Vector2, Vector2>(rect.offset, rect.size));
            bulletPool.Add(bulletName, new List<IBullet>());
        }
        if (bulletPool[bulletName].Any(bullet => !bullet.Active))
            return bulletPool[bulletName].First(bullet => !bullet.Active);
        var newBullet = new Bullet();
        bulletPool[bulletName].Add(newBullet);
        return newBullet;
    }

    void Update()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        foreach (var pool in bulletPool)
        {
            var key = pool.Key;
            var drawn = rendered[key];
            matrices[key].Clear();
            foreach (var bullet in pool.Value)
            {
                if (!bullet.Active) continue;
                var inCamera =
                    GeometryUtility.TestPlanesAABB(planes, new Bounds(bullet.Transform.position, Vector3.one));
                if (!inCamera) continue;
                matrices[key].Add(Matrix4x4.TRS(bullet.Transform.position, bullet.Transform.drawRotation, Vector3.one));
            }
            if (matrices[key].Count < 1024)
                Graphics.DrawMeshInstanced(drawn.Item1, 0, drawn.Item2, matrices[key]);
            else
            {
                foreach (var array in matrices[key].Select((m, i) => Tuple.Create(m, i)).GroupBy(t => t.Item2 / 1024))
                {
                    Graphics.DrawMeshInstanced(drawn.Item1, 0, drawn.Item2, array.Select(g => g.Item1).ToArray());
                }
            }
        }
    }

    void FixedUpdate()
    {
        foreach (var pool in bulletPool)
        {
            var key = pool.Key;
            var rect = box[key];
            foreach (var bullet in pool.Value)
            {
                if (!bullet.Active) continue;
                bullet.Transform.Update();
                HitTest(bullet, rect, bullet.Transform.rotation.eulerAngles.z, bullet.LayerMask);
            }
        }
    }

    void HitTest(IBullet bullet, Tuple<Vector2, Vector2> rect, float rotation, LayerMask layerMask)
    {
        var hitCount = Physics2D.OverlapBoxNonAlloc(bullet.Transform.position + (Vector3) (rotation * rect.Item1),
            rect.Item2, rotation, collisions, layerMask);
        for (var i = 0; i < hitCount; i++)
        {
            bullet.OnTriggerEnter2D(collisions[i]);
            if (!bullet.Active) return;
        }
    }

    public static void DisableAllBullets()
    {
        foreach (var bullet in instance.bulletPool.SelectMany(pool => pool.Value))
        {
            bullet.Active = false;
        }
    }
}