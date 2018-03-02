using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ObjectPool<T> : Singleton<T> where T : ObjectPool<T>
{
    protected abstract string Path(); //Resources 폴더 내부의 경로
    string GetPath(string target) => Path() + target;

    public readonly Dictionary<string, GameObject> originals = new Dictionary<string, GameObject>();
    public readonly Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

    static GameObject GetOriginal(string target)
    {
        if (instance.originals.ContainsKey(target))
        {
            return instance.originals[target];
        }
        var fullPath = instance.GetPath(target);
        var original = Resources.Load<GameObject>(fullPath);
        instance.originals.Add(target, original);
        return original;
    }

    static void CreatePool(string target)
        => instance.pool.Add(target, new List<GameObject>());

    protected virtual void Clear()
    {
        foreach (var kvp in pool)
        {
            foreach (var obj in kvp.Value)
            {
                Destroy(obj);
            }
            kvp.Value.Clear();
        }
    }

    protected virtual GameObject ReadyObject(GameObject obj, GameObject original)
    {
        obj.SetActive(true);
        if (obj.transform.parent != transform)
            obj.transform.SetParent(transform);
        obj.SendMessage("Ready", SendMessageOptions.DontRequireReceiver);
        return obj;
    }

    public GameObject this[string target] => Get(target);

    public static GameObject Get(string target)
    {
        if (!instance.pool.ContainsKey(target)) CreatePool(target);
        var pool = instance.pool[target];
        var original = GetOriginal(target);
        var inactive = pool.FirstOrDefault(o => !o.activeSelf);
        if (inactive != null) return instance.ReadyObject(inactive, original);
        var instantiated = Instantiate(GetOriginal(target));
        pool.Add(instantiated);
        return instance.ReadyObject(instantiated, original);
    }
}