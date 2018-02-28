using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    bool isHit;
    public List<Spawner> spawnerToControl;

    private void Start()
    {
        isHit = false;
    }

    public void StartSpawnByHit()
    {
        if (isHit) return;
        isHit = true;
        StartSpawn();
    }

    void StartSpawn()
    {
        GetComponent<SoundPlayer>().Play(SoundType.Object_Spawner_Enable);
        foreach (var item in spawnerToControl)
        {
            item.StartCoroutine("Spawn");
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
