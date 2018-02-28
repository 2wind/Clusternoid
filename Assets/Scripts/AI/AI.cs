﻿using System;
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
        //nearestCharacter = PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestCharacter(transform.position);
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
        GameObject effect;
        
        if (gameObject.name.Contains("Destructable Door"))
        {
            effect = EffectPool.Get("DoorDestruction");
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            GetComponent<SoundPlayer>().Play(SoundType.Object_Door_Destruct_Start);
            Invoke("PlayDoorFinish", 0.8f);
        }
        else if (gameObject.name.Contains("clonekitbox"))
        {
            effect = EffectPool.Get("RobotExplosion");
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            GetComponent<SoundPlayer>().Play(SoundType.Object_CloneKitBox_Destruct);
        }
        else
        {
            effect = EffectPool.Get("RobotExplosion");
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            GetComponent<SoundPlayer>().Play(SoundType.Enemy_Death);
        }
        ani.SetBool("die", true);
    }

    private void PlayDoorFinish()
    {
        GetComponent<SoundPlayer>().Play(SoundType.Object_Door_Destruct_Finish);
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
        if (PlayerController.groupCenter.characters.Any())
        {
            nearestCharacter = PlayerController.groupCenter.FindNearestCharacter(transform.position);

            Rotation = (Quaternion.Angle(
                transform.rotation,
                Quaternion.FromToRotation(transform.position, nearestCharacter.transform.position)
                )
                );
        }
    }
    
}
