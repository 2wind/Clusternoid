using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowScoreBoardOnClick : MonoBehaviour {

    public GameObject scoreBoard;

	public void ShowScoreBoard()
    {
        scoreBoard.SetActive(true);
    }
}
