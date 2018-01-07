using UnityEngine;

public class Health : MonoBehaviour
{
    public int initialHP = 10;

    int currentHP;
    
    /// <summary>
    /// 공격판정을 당했을 때 호출되는 함수. 공격이 실제로 피해를 입혔다면 true
    /// </summary>
    public bool GetAttack(Attack attack)
    {
        // TODO : 공격을 받아서 피가 달든 넉백을 당하든 알아서 할 것
        currentHP -= attack.damage;
        if (gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            PlayerController.groupCenter.GetComponent<PlayerController>().SendMessage("RemoveCharacter", gameObject);
        }
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
        return true;
    }
}