using System.Collections;
using UnityEngine;
/// <summary>
/// from code of SMZ
/// </summary>
/// 
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour{

    bool isMusicPlayer = false;
    bool isPlayable = true;
    AudioSource audio;

    public float volumeOverride = 1f;
    public bool isVolumeOverrided = false;

    public void SetMusicPlayer(){
        isMusicPlayer = true;
        audio.priority = 10;
    }

    public void SetPlayable(bool set)
    {
        isPlayable = set;
    }

    public void SetVolumeOverride(bool set, float volume)
    {
        isVolumeOverrided = set;
        volumeOverride = volume;
    }

    void Awake(){
        audio = GetComponent<AudioSource>();
    }

    // TODO: PlayOneShot()을 이용해 한 source에서 여러 소리가 나올 수 있도록 하기.
    public void Play(AudioClip clip, bool loop = false)
    {
        if (!isPlayable)
        {
            audio.Stop();
            return;
        }

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
            audio.Stop();
            audio.clip = clip;
            audio.loop = false;
            audio.Play();
        }
        

    }

    public void Play(SoundType soundType, bool loop = false) => Play(SoundManager.GetAudioClip(soundType), loop);
    public void Play(string soundType, bool loop = false) => Play(SoundManager.GetAudioClip(soundType), loop);

    public void Stop() => audio.Stop();

    void Update(){
        if (isVolumeOverrided)
        {
            audio.volume = volumeOverride;
        }
        else
        {
            audio.volume = isMusicPlayer ? SoundManager.musicVolume : SoundManager.soundVolume;
        }
    }
}