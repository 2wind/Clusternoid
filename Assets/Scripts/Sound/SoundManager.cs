using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType { Player_Clone, Player_Footstep, Player_Dash, Player_Hit,
                        Weapon_Single_Fire, Weapon_Single_Hit, Weapon_Shotgun_Fire, Weapon_Shotgun_Hit,
                        Enemy_ShotgunRobot_Fire, Enemy_Burster_Fire, Enemy_Turret_Aim, Enemy_Turret_Fire,
                        Object_Spawner_Enable, Object_Spawner_Disable,
                        UI_Button_Hover, UI_Button_Click }
public enum MusicType { Main, Ingame, GameOver }

[System.Serializable]
public class SoundDic{

    public SoundType type;
    public AudioClip clip;

}
/// <summary>
/// from code of SMZ
/// </summary>
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