using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void SetGameOverPanel(bool value) => StartCoroutine(SetPanelActive(value));

    IEnumerator SetPanelActive(bool value)
    {
        if (value) yield return new WaitForSeconds(2);
        gameOverPanel.SetActive(value);
    }
}