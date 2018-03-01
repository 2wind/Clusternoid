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
        //nearestCharacter = PlayerController.groupCenter.GetComponent<PlayerController>().FindNearestCharacter(transform.position);
    }

    private void Update()
    {
    }



    public void GetAttack()
    {
        ani.SetTrigger("hit");
    }

    public IEnumerator SetDeath()
    {
        GameObject effect;

        if (gameObject.name.Contains("Destructable Door"))
        {
            GetComponentsInChildren<Renderer>()[0].enabled = false;
            GetComponentsInChildren<Renderer>()[1].enabled = false;
            GetComponent<Collider2D>().attachedRigidbody.simulated = false;
            effect = EffectPool.Get("DoorDestruction");
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            GetComponent<SoundPlayer>().Play(SoundType.Object_Door_Destruct_Start);
            yield return new WaitForSeconds(0.8f);
            GetComponent<SoundPlayer>().Play(SoundType.Object_Door_Destruct_Finish);
            yield return new WaitForSeconds(0.2f);
            ani.SetBool("die", true);
        }
        else if (gameObject.name.Contains("clonekitbox"))
        {
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<Collider2D>().attachedRigidbody.simulated = false;
            effect = EffectPool.Get("RobotExplosion");
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            GetComponent<SoundPlayer>().Play(SoundType.Object_CloneKitBox_Destruct);
            yield return new WaitForSeconds(0.8f);
            ani.SetBool("die", true);

        }
        else
        {
            GetComponent<SoundPlayer>().Play(SoundType.Enemy_Death);
            yield return new WaitForSeconds(0.4f);
            ani.SetBool("die", true);
            effect = EffectPool.Get("RobotExplosion");
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }


    }


    private void PlayDoorFinish()
    {
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
