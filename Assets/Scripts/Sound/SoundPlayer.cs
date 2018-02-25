using System.Collections;
using UnityEngine;
/// <summary>
/// from code of SMZ
/// </summary>
/// 
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour{

    bool isMusicPlayer = false;
    AudioSource audio;

    public void SetMusicPlayer(){
        isMusicPlayer = true;
        audio.priority = 10;
    }
    void Awake(){
        audio = GetComponent<AudioSource>();
    }

    // TODO: PlayOneShot()을 이용해 한 source에서 여러 소리가 나올 수 있도록 하기.
    public void Play(AudioClip clip, bool loop = false)
    {
        if (loop || isMusicPlayer)
        {
            if (audio.clip != clip)
            {
                audio.Stop();
            }
            audio.clip = clip;
            audio.loop = loop || isMusicPlayer;
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            audio.PlayOneShot(clip);
        }

    }

    public void Play(SoundType soundType, bool loop = false) => Play(SoundManager.GetAudioClip(soundType), loop);
    public void Play(string soundType, bool loop = false) => Play(SoundManager.GetAudioClip(soundType), loop);

    void Update(){
        audio.volume = isMusicPlayer ? SoundManager.musicVolume : SoundManager.soundVolume;
    }
}