using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class SelectOnInput : MonoBehaviour {


    public EventSystem eventSystem;
    
    public GameObject selectedObject;

    private bool buttonSelected;

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        SceneManager.activeSceneChanged += SetEventSystem;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SetEventSystem;
    }


    void SetEventSystem(Scene from, Scene to)
    {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(Input.GetAxisRaw("Vertical")) > 0.1f && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
