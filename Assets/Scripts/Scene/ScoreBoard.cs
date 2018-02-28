using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


public class ScoreBoard : Singleton<ScoreBoard> {

    public List<ScoreData> scores;
    public ScoreTracker current;
    public GameObject endPanel;
    public Text clearInfoText;
    public bool isMapCleared;

    void OnEnable()
    {
      //  Load();
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
            scores.Add(current.scoreData);
        }
        // esle do nothing. 
    }

    public void ShowClearPanel(string sceneToLoad, bool isInGameScene)
    {
        StopTracking(true);
        endPanel.SetActive(true);
        var clearInfo =
            $"맵 이름: {current.mapName}\n클리어 시간: {Math.Round(current.elapsedTime, 2)}초\n생존한 캐릭터 수: {current.characterCount}명";
        clearInfoText.text = clearInfo;
        Time.timeScale = 0.0f;

    }

    public void HideClearPanel()
    {
        endPanel.SetActive(false);
        Time.timeScale = 1.0f;

    }

    private void OnApplicationQuit()
    {
      //  Save();
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/ScoreData.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ScoreData.gd", FileMode.Open);
            file.Position = 0;
            scores = (List<ScoreData>)bf.Deserialize(file);
            file.Close();
        }
    }

    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/ScoreData.gd");
        bf.Serialize(file, scores);
        file.Close();
    }

    void ClearSavedData()
    {
        if (File.Exists(Application.persistentDataPath + "/ScoreData.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/ScoreData.gd");
            file.Close();
        }
    }
}
