using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour {

    public GameObject pausePanel;
    public bool isOnPause;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !SceneLoader.instance.isMapLoading)
        {
            isOnPause = !isOnPause;
            ScoreBoard.instance.current.Toggle();
        }
        if (SceneLoader.instance.isMapLoading)
        {
            isOnPause = false;
        }

        pausePanel.SetActive(isOnPause);
        if (isOnPause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }


    public void SetPause(bool pause)
    {
        isOnPause = pause;
    }

}
