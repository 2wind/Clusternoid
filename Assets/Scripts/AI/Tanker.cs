using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(MovingAI))]
public class Tanker : MonoBehaviour {

    // 플레이어를 인식하는 +- 각도 in degree
    public float degree = 45;
    private float degreeRadCosine;

    [HideInInspector]
    public MovingAI mAI;

    // Use this for initialization
    void Start () {
        mAI = GetComponent<MovingAI>();
        degreeRadCosine = Mathf.Cos(degree * Mathf.Deg2Rad);
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerController.groupCenter.characters.Any())
        {
            CheckAttack();
        }
		
	}

    void CheckAttack()
    {
        var playerInAttackRange = Physics2D.OverlapCircle(transform.position, mAI.attackDistance, 1 << LayerMask.NameToLayer("Player"));

        if (playerInAttackRange != null)
        {
            var dotProduct = Vector2.Dot(transform.up, (playerInAttackRange.transform.position - transform.position).normalized);
            if (dotProduct > degreeRadCosine)
            {
                // Debug.Log(gameObject.name + " " + dotProduct);
                mAI.attack = true;
            }
            else
            {
                mAI.attack = false;
            }
        }
        else
        {
            mAI.attack = false;
        }
        mAI.ani.SetBool("attack", mAI.attack);
    }
}
