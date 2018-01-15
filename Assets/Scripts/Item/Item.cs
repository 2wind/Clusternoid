using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 아이템을 표현하는 추상 클래스. 실제 구현은 상속으로
/// </summary>
/// 
public abstract class Item : MonoBehaviour {
    /// <summary>
    /// 아이템을 주우면 이 함수가 호출된다
    /// </summary>
    /// <returns></returns>
    public abstract bool Action(GameObject other);
    

}
