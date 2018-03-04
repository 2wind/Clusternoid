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
            var scoreInfo = ScoreBoard.instance.scores.OrderByDescending(score => score.characterCount).ThenBy(score => score.elapsedTime).ToList();
            for (int i = 0; i < Mathf.Min(10, scoreInfo.Count); i++)
            {
                var current = scoreInfo[i];
                if (topTen != null)
                    topTen.text +=
                        $"{current.mapName} | {current.startTime:d} {current.startTime:t} | {System.Math.Round(current.elapsedTime, 2)} | {current.characterCount}\n";
            }
        }

    }

}
