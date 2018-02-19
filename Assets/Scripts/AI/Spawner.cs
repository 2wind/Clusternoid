using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Spawner : MonoBehaviour {

    [Serializable]
    public struct SpawnIndex
    {
        public GameObject item;
        public float possibility;
          
    }

    public float spawnTime = 5f;
    public float delay = 1f;
    AI ai;
    Animator ani;

    public List<SpawnIndex> spawnList;
    float total;
    bool spawned = false;
    
	// Use this for initialization
	void Awake () {
        ai = GetComponent<AI>();
        ani = GetComponentInChildren<Animator>();
        total = spawnList.Select(x => x.possibility).Sum();
    }

    // Update is called once per frame
    void Update () {
       
	}

    IEnumerator Spawn()
    {
        for (int i = 0; i < spawnTime; i++)
        {
            float randomNumber = UnityEngine.Random.Range(0, total);
            SpawnIndex selected = spawnList[0];
            foreach (var item in spawnList)
            {
                if (randomNumber <= item.possibility)
                {
                    selected = item;
                    break;
                }
                randomNumber -= item.possibility;
            }
            Instantiate(selected.item, 
                Clusternoid.Math.RandomOffsetPosition(transform.position, 2f), Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }

    }
}
