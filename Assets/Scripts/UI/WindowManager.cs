using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    void OnEnable()
    {
        UIManager.RegisterWindow(gameObject);
    }

    void OnDisable()
    {
       
    }

    // 창을 닫을 때 SendCancelCommand를 써야 제대로 dequeue가 된다.
    public void SendCancelCommand() => UIManager.CancelAction();


}
