using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 쉽게 열리고 닫히는(== esc로 닫을 수 있는) 창들을 stack으로 관리하기 위한 스크립트
/// - 창을 만들 때: 창에 WindowManager 스크립트를 추가하면 창이 열릴 때 UI매니저에 들어간다.
/// - 창을 닫을 때: 맨 앞의 창을 닫으려면 UIManager.CancelAction이나 WindowManager.SendCancelAction을 부른다.
///      - 창을 닫을 때 특별한 처리가 필요하다면 아래 CancelAction의 switch-case 문에 추가한다.
/// </summary>
public class UIManager : Singleton<UIManager>
{

    public Stack<GameObject> windows;
    public PausePanel pausePanel;

    public static Stack<GameObject> ActiveWindows => instance.windows;
    public static PausePanel PausePanel => instance.pausePanel;

	// Use this for initialization
	void Start ()
	{
		windows = new Stack<GameObject>();
	    pausePanel = PauseMananger.instance.gameObject.GetComponent<PausePanel>();
	}

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !SceneLoader.instance.isMapLoading)
        {
            CancelAction();
        }
    }

    public static void RegisterWindow(GameObject window)
    {
        ActiveWindows.Push(window);
       // Debug.Log(window.name + ActiveWindows.Count);
    }

    public static GameObject ShowTopWindow()
    {
        return ActiveWindows.Peek();
    }



    public static void CancelAction()
    {
        if (ActiveWindows.Count == 0)
        {
            //Open cancel window
            PausePanel.SetPanel(true);
        }
        else
        {

            var lastWindow = ActiveWindows.Pop();
            //Debug.Log(lastWindow.name + ActiveWindows.Count);
            switch (lastWindow.name)
            {
                // 패널을 닫을 때 특별한 처리가 필요한 경우 여기에서 처리를 해준다.
                // 특별한 처리를 할 때, 창이 닫혀야 한다.
                    case "PausePanel":
                        PausePanel.SetPanel(false);
                        break;
                    default:
                        lastWindow.SetActive(false);
                        break;
            }
            //lastWindow.SendMessage();
            //lastWindow.close();
        }
    }
}
