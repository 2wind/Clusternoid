using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class WeaponAdder : MonoBehaviour {


    int random;
    Weapon weapon;

	// Use this for initialization
	void OnEnable() {
        random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                gameObject.AddComponent<Pistol>();
                GetComponent<Pistol>().preShoot = Clusternoid.Math.NextGaussian(80, 1, 50, 100);
                break;

            case 1:
                gameObject.AddComponent<Shotgun>();
                GetComponent<Shotgun>().preShoot = Clusternoid.Math.NextGaussian(3, 1, 1.5f, 4.5f);
                GetComponent<Shotgun>().angle = 15;
                break;

            case 2:
                gameObject.AddComponent<Burst>();
                GetComponent<Burst>().preShoot = Clusternoid.Math.NextGaussian(3, 1, 1.5f, 4.5f);
                GetComponent<Burst>().burst = 3;
                GetComponent<Burst>().delay = 0.3f;
                break;

            default:
                gameObject.AddComponent<Pistol>();
                GetComponent<Pistol>().preShoot = 100f;
                if (Debug.isDebugBuild)
                    Debug.LogError("WeaponAdder Random value error, random: " + random);
                break;
        }
        weapon = GetComponent<Weapon>();
        weapon.firingPosition = transform.Find("Firing Position");
        if (weapon.firingPosition == null)
        {
            if (Debug.isDebugBuild)
                Debug.LogError("Firing Position not found in this object.");
        }
        weapon.damage = 1;
        weapon.bulletSpeed = 25;
        weapon.spread = 0.5f;
        if (PlayerController.groupCenter.emittingCount > 5)
        {
            weapon.isEmittingSound = false;
        }
        else
        {
            PlayerController.groupCenter.emittingCount += 1;
          //  Debug.Log(PlayerController.groupCenter.emittingCount);
            weapon.isEmittingSound = true;
        }
    }

    private void OnDisable()
    {
        if (weapon.isEmittingSound)
        {
            var mute = PlayerController.groupCenter.characters.FindAll(ch => ch.GetComponent<Weapon>().isEmittingSound == false);
            if (mute.Any())
            {
               mute[Random.Range(0, mute.Count)].GetComponent<Weapon>().isEmittingSound = true;
            }
            else
            {
                PlayerController.groupCenter.emittingCount -= 1;
            }
        }
        Destroy(GetComponent<Weapon>());
    }
}