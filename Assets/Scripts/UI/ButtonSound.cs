using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour {

	public void PlayHover()
    {
   //     Debug.Log("Hover");
        SoundManager.Play(SoundType.UI_Button_Hover);
    }

    public void PlayClick()
    {
   //     Debug.Log("Click");
        SoundManager.Play(SoundType.UI_Button_Click);
    }




}
