using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour {

    public GameObject pausePanel;
    public bool isPanelActive;
    private Guid _guid;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !SceneLoader.instance.isMapLoading)
        {
            TogglePanel();
        }

    }

    private void TogglePanel()
    {
        isPanelActive = !isPanelActive;
        ScoreBoard.instance.current.Toggle();

        if (SceneLoader.instance.isMapLoading)
        {
            isPanelActive = false;
        }
        if (isPanelActive)
        {
            _guid = PauseMananger.Pause();
        }
        else
        {
            PauseMananger.Resume(_guid);
        }

        pausePanel.SetActive(isPanelActive);
    }
}
