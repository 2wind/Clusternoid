using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectPool 등에 들어가는 오브젝트가, 자기 자신을 N초 후 비활성화(원래라면 Destroy)해야 하는 경우에 
/// 사용한다. 일반적인 오브젝트면 Destroy(gameObject, time)을 하면 되지만 SetActive에는 그런 함수가
/// 없어서 만들었다.
/// </summary>
public class SelfDisabler : MonoBehaviour { 

    public float time = 1f;

    private void OnEnable()
    {
        Invoke(nameof(DisableItself), time);
    }

    void DisableItself()
    {
        gameObject.SetActive(false);
    }
}
