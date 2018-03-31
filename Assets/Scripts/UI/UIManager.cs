using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    public Queue<GameObject> activeWindows;

	// Use this for initialization
	void Start () {
		activeWindows = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
