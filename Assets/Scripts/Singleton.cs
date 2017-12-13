using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance
    => _instance ?? (_instance = CreateNew());
    static T _instance;
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = (T)this;
    }
    static T CreateNew()
    {
        var go = new GameObject(nameof(T));
        return go.AddComponent<T>();
    }
}
