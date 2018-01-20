using UnityEngine;

public class Health : MonoBehaviour
{
    public int initialHP = 10;

    int currentHP;

    private void Awake()
    {
        currentHP = initialHP;
    }

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
            //TODO: 실제로 destroy하지는 말고, 시체는 남겨 두어야 할 것. 이를 위해 시체 로직을 만들어야 한다.
        }else if (currentHP <= 0)
        {
            GetComponent<DropItem>()?.Drop();
            Destroy(gameObject);
        }
        return true;
    }
}