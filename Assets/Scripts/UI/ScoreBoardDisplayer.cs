using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScoreBoardDisplayer : MonoBehaviour {

    public Text topTen;

    private void Awake()
    {
        if (ScoreBoard.instance.scores != null)
        {
            var scoreInfo = ScoreBoard.instance.scores.OrderBy(score => score.elapsedTime).ToList();
            for (int i = 0; i < Mathf.Min(10, scoreInfo.Count()); i++)
            {
                var current = scoreInfo[i];
                topTen.text += System.String.Format("{0} | {1:d} {1:t} | {2} | {3}\n", current.mapName, current.startTime, System.Math.Round(current.elapsedTime, 2), current.characterCount);
            }
        }

    }

}
