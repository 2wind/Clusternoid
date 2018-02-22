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
public class SoundManager : MonoBehaviour{
    static SoundManager instance;
    static Queue<SoundPlayer> spPool;
    static SoundPlayer musicPlayer;
    
    public GameObject standardSoundPlayer;
    public SoundDic[] soundDictionary;
    public AudioClip[] musicDictionary;
    static SoundPlayer GetSoundPlayer(){
        if (spPool.Count > 0) {
            SoundPlayer sp = spPool.Dequeue();
            sp.gameObject.SetActive(true);
            return sp;
        } else {
            GameObject go = Instantiate(instance.standardSoundPlayer, instance.gameObject.transform);
            return go.GetComponent<SoundPlayer>();
        }
    }
    public static void PushUsedSoundPlayer(SoundPlayer sp) { 
        spPool.Enqueue(sp);
    }

    public void PlayNonStatic(SoundType st){
        SoundManager.Play(st);
    }
    public static void Play(SoundType st){
        var selectedDics = instance.soundDictionary.Where(sd => sd.type == st).ToArray();
        var clip = selectedDics[Random.Range(0,selectedDics.Length)].clip;
        GetSoundPlayer().Play(clip);
    }
    public static void Play(MusicType mt){
        int n;
        bool loop = true;
        switch(mt){
            case MusicType.Main:        n = 0;                  break;
            case MusicType.Ingame:      n = 1;                  break;
            case MusicType.GameOver:    n = 2; loop = false;    break;
            default:                    n = 0;                  break;
        }
        var clip = instance.musicDictionary[n];
        musicPlayer.Play(clip, loop);
    }

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            spPool = new Queue<SoundPlayer>();
            var musicPlayerGO = Instantiate(instance.standardSoundPlayer, instance.gameObject.transform);
            musicPlayer = musicPlayerGO.GetComponent<SoundPlayer>();
            musicPlayer.SetMusicPlayer();
            DontDestroyOnLoad(instance);
        }
    }
}