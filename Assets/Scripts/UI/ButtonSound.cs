using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour {

	public void PlayHover()
    {
        SoundManager.Play(SoundType.UI_Button_Hover);
    }

    public void PlayClick()
    {
        SoundManager.Play(SoundType.UI_Button_Click);
    }
}
