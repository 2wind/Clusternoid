using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {

    public GameObject gameOverPanel;

    public void SetGameOverPanel(bool value) => gameOverPanel.SetActive(value);

}
