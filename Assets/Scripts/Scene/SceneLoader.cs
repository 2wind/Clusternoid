using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {

    // 일단 씬은 한 번에 하나만 로드된다고 가정한다.
    // 매니저 씬 제외.
   // [HideInInspector]
    public string currentLoadedScene;
    [HideInInspector]
    public bool isLoadedSceneInGame = false;
    public GameObject groupCenter;
    public GameObject loadingPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject mainVCamera;

    [HideInInspector]
    public bool isMapLoading = false;

    [HideInInspector] public bool firstRun = true;

    private void Start()
    {
    #if UNITY_EDITOR
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (!SceneManager.GetSceneAt(i).name.Equals("manager scene"))
                {
                    currentLoadedScene = SceneManager.GetSceneAt(i).name;
                }

            }
            if (string.IsNullOrEmpty(currentLoadedScene))
            {
                LoadScene("Opening", false);
            }
            else if (currentLoadedScene != "Opening" && Debug.isDebugBuild)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLoadedScene));
            isLoadedSceneInGame = true;
            groupCenter.GetComponent<PlayerController>().Initialize();
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(currentLoadedScene));
        }
#else
          LoadScene("Opening", false);
#endif

    }
    public void LoadScene(string name, bool isInGame = true)
    {

        StartCoroutine(LoadSceneAsync(name, isInGame));
    }

    // TODO: IEnumerator를 이용해 스무스하고 모던하고 어고노미컬한 로딩을 세팅
    IEnumerator LoadSceneAsync(string name, bool isInGame)
    {

        if (!Application.CanStreamedLevelBeLoaded(name))
        {
            Debug.LogError("Scene not found. name: " + name);
            isMapLoading = false;
            yield break;
        }
        loadingPanel.GetComponent<CanvasGroup>().alpha = 1;
        loadingPanel.SetActive(true);
        isMapLoading = true;
        gameObject.GetComponent<PausePanel>().SetPanel(false);
        CleanUp();
        currentLoadedScene = name;

        var loading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLoadedScene));
        isLoadedSceneInGame = isInGame;

        if (isInGame)
        {
            groupCenter.GetComponent<PlayerController>().Initialize();
        }
        else
        {
            groupCenter.GetComponent<PlayerController>().Clean();
        }
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(currentLoadedScene));
        isMapLoading = false;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            loadingPanel.GetComponent<CanvasGroup>().alpha = 1 - i;
            yield return null;
        }
        loadingPanel.SetActive(false);

    }


    public void ReloadScene()
    {
        groupCenter.GetComponent<PlayerController>().Clean();
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
        if (!string.IsNullOrEmpty(currentLoadedScene))
        {
            SceneManager.UnloadSceneAsync(currentLoadedScene);
        }
        BulletPool.DisableAllBullets();

    }
}
