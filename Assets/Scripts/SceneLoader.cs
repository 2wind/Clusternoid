using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {

    // 일단 씬은 한 번에 하나만 로드된다고 가정한다.
    // 매니저 씬 제외.
    string currentLoadedScene;
    public GameObject groupCenter;
    public GameObject cursor;

    private void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!SceneManager.GetSceneAt(i).name.Equals("manager scene"))
            {
                currentLoadedScene = SceneManager.GetSceneAt(i).name;
            }

        }
    }
    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }

    // TODO: IEnumerator를 이용해 스무스하고 모던하고 어고노미컬한 로딩을 세팅
    IEnumerator LoadSceneAsync(string name)
    {
        CleanUp();
        currentLoadedScene = name;
        var loading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLoadedScene));
        groupCenter.GetComponent<PlayerController>().Initialize();
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(currentLoadedScene));
    }


    public void ReloadScene()
    {
        LoadScene(currentLoadedScene);
    }

    void CleanUp()
    {
        //if (groupCenter.activeInHierarchy)
        //{
        //    var chs = PlayerController.groupCenter.characters;
        //    foreach (var ch in chs)
        //    {
        //        ch.transform.position = PlayerController.groupCenter.transform.position;
        //    }
        //}

        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("manager scene"));
        if (currentLoadedScene != null)
        {
            SceneManager.UnloadSceneAsync(currentLoadedScene);
        }
    }
}
