using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject bullet;
    public GameObject healthPack;
    public GameObject player; //현재 플레이어. 나중에 코옵을 지원하게 되면 리스트가 되겠지?


	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
	
}
