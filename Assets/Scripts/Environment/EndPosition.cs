using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPosition : MonoBehaviour {


    public string sceneToLoad;
    public bool isInGameScene = true;

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + Vector3.back, "Triggers/end.png", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreBoard.instance.ShowClearPanel(sceneToLoad, isInGameScene);
            GetComponent<Collider2D>().enabled = false;
        }
    }
    
}
