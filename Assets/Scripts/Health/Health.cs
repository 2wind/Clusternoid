using UnityEngine;

public class Health : MonoBehaviour
{
    /// <summary>
    /// 공격판정을 당했을 때 호출되는 함수. 공격이 실제로 피해를 입혔다면 true
    /// </summary>
    public bool GetAttack(Attack attack)
    {
        // TODO : 공격을 받아서 피가 달든 넉백을 당하든 알아서 할 것
        return true;
    }
}