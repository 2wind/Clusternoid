using System.Collections;
using UnityEngine;
/// <summary>
/// from code of SMZ
/// </summary>
public class SoundPlayer : MonoBehaviour{

    bool isMusicPlayer = false;
    AudioSource audio;

    public void SetMusicPlayer(){
        isMusicPlayer = true;
        audio.priority = 256;
    }
    void Awake(){
        audio = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, bool loop = false)
    {
        audio.clip = clip;
        audio.loop = loop || isMusicPlayer;
        audio.Play();
    }

    public void Play(SoundType soundType, bool loop = false) => Play(SoundManager.GetAudioClip(soundType), loop);

    void Update(){
        audio.volume = isMusicPlayer ? SoundManager.musicVolume : SoundManager.soundVolume;
    }
}