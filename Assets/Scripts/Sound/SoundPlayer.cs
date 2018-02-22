using System.Collections;
using UnityEngine;
/// <summary>
/// from code of SMZ
/// </summary>
public class SoundPlayer : MonoBehaviour{
    public static float soundVolume = 1;
    public static float musicVolume = 1;
    bool isMusicPlayer = false;
    AudioSource audio;

    public void SetMusicPlayer(){
        isMusicPlayer = true;
    }
    void Awake(){
        audio = GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip, bool loop = false){
        audio.Stop();
        audio.clip = clip;
        audio.Play();
        if (isMusicPlayer) audio.loop = loop;
        else StartCoroutine(PushThisToPool(clip.length));
    }   
    IEnumerator PushThisToPool(float length){
        yield return new WaitForSeconds(length);
        SoundManager.PushUsedSoundPlayer(this);
        gameObject.SetActive(false);
    }
    void Update(){
        audio.volume = isMusicPlayer? musicVolume : soundVolume;
    }
}