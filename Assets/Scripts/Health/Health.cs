using UnityEngine;

[RequireComponent(typeof(HitListener))]
public class Health : MonoBehaviour
{
    public int initialHP = 10;

    [HideInInspector] public int currentHP;

    Animator ani;
    IAttackListener listener;

    void OnEnable()
    {
        currentHP = initialHP;
        var ai = GetComponent<AI>();
        listener = ai == null ? (IAttackListener) new PlayerAttackListener(this) : new AiAttackListener(this, ai);
        ani = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 공격판정을 당했을 때 호출되는 함수. 공격이 실제로 피해를 입혔다면 true
    /// </summary>
    public bool GetAttack(Attack attack)
    {
        return listener.GetAttack(attack);
    }

    interface IAttackListener
    {
        bool GetAttack(Attack attack);
    }

    class PlayerAttackListener : IAttackListener
    {
        bool dead = false;
        readonly Health health;

        public PlayerAttackListener(Health health)
        {
            this.health = health;
        }

        public bool GetAttack(Attack attack)
        {
            health.currentHP -= attack.damage;
            if (!dead)
                PlayerController.groupCenter.RemoveCharacter(health.GetComponent<Character>());
            dead = true;
            if (health.currentHP > 0)
            {
                var effect = EffectPool.Get("PlayerHit");
                effect.transform.position = health.transform.position;
                effect.SetActive(true);
            }
            else
            {
                var effect = EffectPool.Get("PlayerExplosion");
                effect.transform.position = health.transform.position;
                effect.SetActive(true);
                health.ani.Play("Idle", 0);
                health.ani.Update(Time.deltaTime);
                health.gameObject.SetActive(false);
            }
            return true;
        }
    }

    class AiAttackListener : IAttackListener
    {
        bool dead = false;
        readonly Health health;
        readonly AI ai;

        public AiAttackListener(Health health, AI ai)
        {
            this.health = health;
            this.ai = ai;
        }

        public bool GetAttack(Attack attack)
        {
            if (dead) return false;
            // TODO : 공격을 받아서 피가 달든 넉백을 당하든 알아서 할 것
            health.currentHP -= attack.damage;

            ai?.GetAttack();
            if (health.currentHP > 0) return true;
            dead = true;
            ai?.StartCoroutine(ai.SetDeath());
            return true;
        }
    }
}