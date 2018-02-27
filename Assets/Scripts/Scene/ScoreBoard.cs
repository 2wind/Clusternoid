using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ScoreBoard : Singleton<ScoreBoard> {

    public List<ScoreTracker> scores;
    public ScoreTracker current;
    public GameObject endPanel;
    public bool isMapCleared;

    void Start()
    {
        // 스코어를 불러온다.
        current = gameObject.AddComponent<ScoreTracker>();
    }

    public void StartNewTracking()
    {
        isMapCleared = false;
        current.StartTracking();
    }

    public void StopTracking(bool cleared = false)
    {
        if (current.isTracking)
        {
            isMapCleared = cleared;
            current.StopTracking(cleared);
            scores.Add(current);
        }
        // esle do nothing. 
    }

    public void ShowClearPanel(string sceneToLoad, bool isInGameScene)
    {
        StopTracking(true);
        endPanel.SetActive(true);
        var clearInfo = String.Format("맵 이름: {0}\n클리어 시간: {1}초\n생존한 캐릭터 수: {2}명",
            current.mapName, Math.Round(current.elapsedTime, 2), current.characterCount);
        endPanel.transform.Find("ClearInfo").GetComponent<Text>().text = clearInfo;
        Time.timeScale = 0.0f;

    }

    public void HideClearPanel()
    {
        endPanel.SetActive(false);
        Time.timeScale = 1.0f;

    }

    private void OnApplicationQuit()
    {
        // 스코어를 저장한다.
    }
}
