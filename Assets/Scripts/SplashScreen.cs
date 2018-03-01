using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public string SceneName;

    private SpriteRenderer LogoIcon1;
    private SpriteRenderer LogoIcon2;
    private SpriteRenderer LogoIcon3;
    private SpriteRenderer LogoIcon4;
    private SpriteRenderer LogoFace;
    private SpriteRenderer LogoWord1;
    private SpriteRenderer LogoWord2;
    private SpriteRenderer LogoWord3;
    private SpriteRenderer LogoName;
    private GameObject Panel;

    private float OpeningTime;
    private bool OpeningOnGoing;

    private void Awake()
    {
        LogoFace = GameObject.Find("LogoFace").GetComponent<SpriteRenderer>();
        LogoIcon1 = GameObject.Find("LogoIcon1").GetComponent<SpriteRenderer>();
        LogoIcon2 = GameObject.Find("LogoIcon2").GetComponent<SpriteRenderer>();
        LogoIcon3 = GameObject.Find("LogoIcon3").GetComponent<SpriteRenderer>();
        LogoIcon4 = GameObject.Find("LogoIcon4").GetComponent<SpriteRenderer>();
        LogoWord1 = GameObject.Find("LogoWord1").GetComponent<SpriteRenderer>();
        LogoWord2 = GameObject.Find("LogoWord2").GetComponent<SpriteRenderer>();
        LogoWord3 = GameObject.Find("LogoWord3").GetComponent<SpriteRenderer>();
        LogoName = GameObject.Find("LogoName").GetComponent<SpriteRenderer>();
        Panel = GameObject.Find("Panel");
    }

    void Start()
    {
        OpeningOnGoing = true;

        LogoIcon1.color = new Color(1, 1, 1, 0);
        LogoIcon2.color = new Color(1, 1, 1, 0);
        LogoIcon3.color = new Color(1, 1, 1, 0);
        LogoIcon4.color = new Color(1, 1, 1, 0);
        LogoFace.color = new Color(1, 1, 1, 0);
        LogoWord1.color = new Color(1, 1, 1, 0);
        LogoWord2.color = new Color(1, 1, 1, 0);
        LogoWord3.color = new Color(1, 1, 1, 0);
        LogoName.color = new Color(1, 1, 1, 0);

        OpeningTime = 0;
    }

    void Update()
    {
        OpeningTime = OpeningTime + Time.deltaTime;

        if (OpeningTime >= 6f)
            OpeningOnGoing = false;

        if (OpeningOnGoing == true)
        {
            LogoFace.color = new Color(1, 1, 1, OpeningTime);

            if (OpeningTime >= 1f)
            {
                LogoIcon1.color = new Color(1, 1, 1, 1);
            }
            if (OpeningTime >= 1.5f)
            {
                LogoIcon2.color = new Color(1, 1, 1, 1);
            }
            if (OpeningTime >= 2f)
            {
                LogoIcon3.color = new Color(1, 1, 1, 1);
            }
            if (OpeningTime >= 2.5f)
            {
                LogoIcon4.color = new Color(1, 1, 1, 1);
            }
            if (OpeningTime >= 3f)
            {
                LogoWord1.color = new Color(1, 1, 1, OpeningTime - 3f);
            }
            if (OpeningTime >= 3.5f)
            {
                LogoWord2.color = new Color(1, 1, 1, OpeningTime - 3.5f);
            }
            if (OpeningTime >= 4f)
            {
                LogoWord3.color = new Color(1, 1, 1, OpeningTime - 4f);
            }
            if (OpeningTime >= 4.5f)
            {
                Panel.GetComponent<Transform>().position = new Vector3(Panel.GetComponent<Transform>().position.x + (OpeningTime / 60), Panel.GetComponent<Transform>().position.y, 0);
                LogoName.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (OpeningTime / 1.5f) - 3f);
            }
        }
        else
        {
            LogoIcon1.color = new Color(1, 1, 1, 1);
            LogoIcon2.color = new Color(1, 1, 1, 1);
            LogoIcon3.color = new Color(1, 1, 1, 1);
            LogoIcon4.color = new Color(1, 1, 1, 1);
            LogoFace.color = new Color(1, 1, 1, 1);
            LogoWord1.color = new Color(1, 1, 1, 1);
            LogoWord2.color = new Color(1, 1, 1, 1);
            LogoWord3.color = new Color(1, 1, 1, 1);
            LogoName.color = new Color(1, 1, 1, 1);
            Panel.SetActive(false);
        }
    }

    public void SkipLogo()
    {
        if (OpeningOnGoing == true)
        {
            OpeningOnGoing = false;
        }
        else
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
