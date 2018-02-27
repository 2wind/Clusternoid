using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreTracker : MonoBehaviour {

    public DateTime startTime;
    public string mapName;
    public float elapsedTime;
    public int characterCount;
    public bool clear;

    public bool isTracking = false;

    public void Start()
    {
        isTracking = false;
    }

    public void StartTracking()
    {
        startTime = DateTime.Now;
        mapName = SceneLoader.instance.currentLoadedScene;
        elapsedTime = 0;
        characterCount = PlayerController.groupCenter.characters.Count;
        clear = false;
        isTracking = true;
    }

    public void StopTracking(bool isCleared)
    {
        isTracking = false;
        clear = isCleared;
        characterCount = PlayerController.groupCenter.characters.Count;
    }

    public void Toggle() => isTracking = !isTracking;
    public void Pause() => isTracking = false;
    public void Resume() => isTracking = true;




    private void Update()
    {
        if (isTracking)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
