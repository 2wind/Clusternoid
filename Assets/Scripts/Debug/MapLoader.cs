using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLoader : MonoBehaviour {

    InputField inputField;
    string sceneName;


    private void Start()
    {
        inputField = transform.Find("InputField").GetComponent<InputField>();
    }


    private void OnGUI()
    {
        sceneName = inputField.text;
    }
    public void LoadScene()
    {
        SceneLoader.instance.LoadScene(sceneName);
    }




}
