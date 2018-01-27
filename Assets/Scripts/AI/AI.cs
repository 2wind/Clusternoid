using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour {
    
    Animator ani;
    public float Rotation { get; set; }
    public Vector2 direction;
    public Character nearestCharacter;

    private void Start()
    {
        ani = GetComponentInChildren<Animator>();
        direction = UnityEngine.Random.insideUnitCircle;
        ChooseRotation();
        nearestCharacter = PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestCharacter(transform.position);
    }

    private void Update()
    {
    }



    public void GetAttack()
    {
        ani.SetTrigger("hit");
    }

    public void SetDeath()
    {
        ani.SetTrigger("die");
    }

    public void ChooseRotation()
    {
        Rotation = (2 * UnityEngine.Random.Range(0, 2) - 1) * 90; // -90 or 90
    }

    public void ChooseDirection()
    {
        direction = UnityEngine.Random.insideUnitCircle;
    }

    public void FindNearestCharacter()
    {
        nearestCharacter = PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestCharacter(transform.position);
  //      Debug.Log(nearestCharacter);
        Rotation = (Quaternion.Angle(
            transform.rotation,
            Quaternion.FromToRotation(transform.position, nearestCharacter.transform.position)
            )
            );
    }
    
}
