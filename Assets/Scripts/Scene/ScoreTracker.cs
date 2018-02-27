using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScoreData
{
    public DateTime startTime;
    public string mapName;
    public float elapsedTime;
    public int characterCount;
    public bool clear;

    public ScoreData()
    {
        startTime = DateTime.Now;
        mapName = SceneLoader.instance.currentLoadedScene;
        elapsedTime = 0;
        characterCount = 1;
        clear = false;
    }
}

public class ScoreTracker : MonoBehaviour {



    public DateTime startTime => scoreData.startTime;
    public string mapName => scoreData.mapName;
    public float elapsedTime => scoreData.elapsedTime;
    public int characterCount => scoreData.characterCount;
    public bool clear => scoreData.clear;

    public bool isTracking = false;
    public ScoreData scoreData;

    public void Start()
    {
        isTracking = false;
    }

    public void StartTracking()
    {
        scoreData = new ScoreData();
        isTracking = true;
    }

    public void StopTracking(bool isCleared)
    {
        isTracking = false;
        scoreData.clear = isCleared;
        scoreData.characterCount = PlayerController.groupCenter.characters.Count;
    }

    public void Toggle() => isTracking = !isTracking;
    public void Pause() => isTracking = false;
    public void Resume() => isTracking = true;




    private void Update()
    {
        if (scoreData != null && isTracking)
        {
            scoreData.elapsedTime += Time.deltaTime;
        }
    }
}
