using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour {


    class CharacterInfo
    {
        public Character character;
        public Vector3 from;
        public Vector3 to;

        public CharacterInfo(Character ch, Vector3 t)
        {
            character = ch;
            from = ch.transform.position;
            to = t;
        }
    }
    List<CharacterInfo> characterInfo;
    CanvasGroup canvas;
    bool openingRunning = true;
    Light spotlight;

	// Use this for initialization
	void Awake () {
        characterInfo = new List<CharacterInfo>();
        spotlight = FindObjectOfType<Light>();
        canvas = FindObjectOfType<CanvasGroup>();
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        StartCoroutine(Initialize());
	}
	
    IEnumerator Initialize()
    {
        var newChr = new CharacterInfo(AddCharacter(new Vector3(0, 28, 0), 180), new Vector3(0, -16, 0));
        characterInfo.Add(newChr);
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 10; i++)
        {
            characterInfo.Add(new CharacterInfo(
                AddCharacter(Clusternoid.Math.RandomOffsetPosition(new Vector3(0, 28, 0), 4), 180 + Random.Range(-25, 25)),
                Clusternoid.Math.RandomOffsetPosition(new Vector3(0, -16, 0), 4)));
        }
        for (int i = 0; i < 10; i++)
        {
            characterInfo.Add(new CharacterInfo(
                AddCharacter(Clusternoid.Math.RandomOffsetPosition(new Vector3(-28, 0, 0), 4), 90 + Random.Range(-25, 25)),
                Clusternoid.Math.RandomOffsetPosition(new Vector3(28, 0, 0), 4)));
        }
        for (int i = 0; i < 10; i++)
        {
            characterInfo.Add(new CharacterInfo(
                AddCharacter(Clusternoid.Math.RandomOffsetPosition(new Vector3(28, 0, 0), 4), 270 + Random.Range(-25, 25)),
                Clusternoid.Math.RandomOffsetPosition(new Vector3(-28, 0, 0), 4)));
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (float f = 0; f < 1; f += Time.deltaTime)
        {
            canvas.alpha = f;
            yield return null;
        }
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    public void StartGame(string name) => StartCoroutine(IStartGame(name));

    IEnumerator IStartGame(string name)
    {
        SoundManager.Play(SoundType.UI_Button_Start);
        openingRunning = false;
        yield return new WaitForSeconds(1);
        characterInfo[Random.Range(0, characterInfo.Count)].character.KillCharacter();
        yield return new WaitForSeconds(1.5f);
        spotlight.enabled = false;
        foreach (var item in characterInfo)
        {
            item.character.KillCharacter();
        }
        yield return new WaitForSeconds(2.5f);
        foreach (var item in characterInfo)
        {
            item.character.gameObject.SetActive(false);
        }
        SceneLoader.instance.LoadScene(name);
    }


    private void Update()
    {
        if (openingRunning)
        {
            foreach (var item in characterInfo)
            {
                item.character.transform.position = Vector3.MoveTowards(item.character.transform.position, item.to, 2.5f * Time.deltaTime);
                item.character.gameObject.GetComponentInChildren<Animator>().SetFloat("velocity", 1);
                if (Vector3.Distance(item.character.transform.position, item.to) < 0.5)
                {
                    item.character.transform.position = item.@from + (Vector3)Random.insideUnitCircle;
                }
            }
        }
        else
        {
            foreach (var item in characterInfo)
            {
                item.character.gameObject.GetComponentInChildren<Animator>()?.SetFloat("velocity", 0);
            }
        }
    }

    public Character AddCharacter(Vector3 position, float rotation)
    {
        var newCharacterGO = CharacterPool.Get("character");
        newCharacterGO.GetComponent<SoundPlayer>().SetPlayable(false);
        newCharacterGO.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotation));
        var newCharacter = newCharacterGO.GetComponent<Character>();
        //newCharacterGO.GetComponent<SoundPlayer>().SetPlayable(true);
        return newCharacter;
    }

    private void OnDisable()
    {
        foreach(var item in characterInfo)
        {
            if (item.character != null)
            {
                item.character.gameObject.GetComponentInChildren<Animator>()?.SetFloat("velocity", 0);
                item.character.GetComponent<SoundPlayer>().SetPlayable(true);
                item.character.transform.position = Vector3.zero;
                item.character.KillCharacter();
            }
        }
    }


}
