using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    Health health;
    public List<Spawner> spawnerToControl;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void StartSpawn()
    {
        if (health.currentHP == health.initialHP)
        {
            foreach (var item in spawnerToControl)
            {
                item.StartCoroutine("Spawn");
            }
        }
    }

    public void EndSpawn()
    {
        foreach (var item in spawnerToControl)
        {
            item.StopCoroutine("Spawn");
        }
    }
}
