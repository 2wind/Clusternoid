using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum SoundType { Player_Clone, Player_Footstep, Player_Dash, Player_Hit,
                        Weapon_Single_Fire, Weapon_Single_Hit, Weapon_Shotgun_Fire, Weapon_Shotgun_Hit,
                        Enemy_ShotgunRobot_Fire, Enemy_Burster_Fire, Enemy_Turret_Aim, Enemy_Turret_Fire,
                        Object_Spawner_Enable, Object_Spawner_Disable,
                        UI_Button_Hover, UI_Button_Click, UI_Button_Start,
                        Object_Door_Destruct_Start, Object_Door_Destruct_Finish, Object_CloneKitBox_Destruct,
                        Enemy_Death
                        }
public enum MusicType { Main, Ingame, GameOver }

[System.Serializable]
public class SoundDic{

    public SoundType type;
    public AudioClip clip;
    [Range(0, 256)] public int priority = 128;
    [Range(0, 1)] public float volume = 1;
    [Range(-3, 3)] public float pitch = 1;

}
/// <summary>
/// from code of SMZ
/// </summary>
[CustomEditor(typeof(SoundManager))]
[CanEditMultipleObjects]
public class SoundManager : Singleton<SoundManager>{

    public static float soundVolume = 1;
    public static float musicVolume = 1;

    static SoundPlayer musicPlayer;
    static SoundPlayer soundPlayer;
    
    public SoundDic[] soundDictionary;
    public AudioClip[] musicDictionary;

    public static AudioClip GetAudioClip(SoundType type)
    {
        return instance.soundDictionary.ToList().Find(dic => dic.type.Equals(type)).clip;
    }
    public static AudioClip GetAudioClip(string type)
    {
        try
        {
            var soundType = (SoundType)System.Enum.Parse(typeof(SoundType), type);
            return GetAudioClip(soundType);
        }
        catch (System.ArgumentException)
        {
            if (Debug.isDebugBuild)
                Debug.LogError(type + " 은(는) 올바른 SoundType이 아닙니다.");
            throw;
        }
    }

    public static SoundDic GetSoundInfo(SoundType type)
    {
        return instance.soundDictionary.ToList().Find(dic => dic.type.Equals(type));
    }
    public static SoundDic GetSoundInfo(string type)
    {
        try
        {
            var soundType = (SoundType)System.Enum.Parse(typeof(SoundType), type);
            return GetSoundInfo(soundType);
        }
        catch (System.ArgumentException)
        {
            if (Debug.isDebugBuild)
                Debug.LogError(type + " 은(는) 올바른 SoundType이 아닙니다.");
            throw;
        }
    }

    public static int GetPriority(SoundType type)
    {
        return instance.soundDictionary.ToList().Find(dic => dic.type.Equals(type)).priority;
    }

    public static void Play(SoundType soundType) => soundPlayer.Play(soundType);
    
    void Start() {
        var musicPlayerGO = transform.Find("MusicPlayer");
        musicPlayerGO.transform.position = (Camera.main.transform.position);
        musicPlayer = musicPlayerGO.GetComponent<SoundPlayer>();
        musicPlayer.SetMusicPlayer();
        musicPlayer.Play(musicDictionary[(int)MusicType.Main]);

        var soundPlayerGO = transform.Find("SoundPlayer");
        soundPlayerGO.transform.position = Camera.main.transform.position;
        soundPlayer = soundPlayerGO.GetComponent<SoundPlayer>();
    }
    
}