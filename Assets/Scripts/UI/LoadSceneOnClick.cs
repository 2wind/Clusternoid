using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadLevel(string name)
    {
        SceneLoader.instance.LoadScene(name);
    }

    public void LoadNonGameLevel(string name)
    {
        SceneLoader.instance.LoadScene(name, false);
    }
}
