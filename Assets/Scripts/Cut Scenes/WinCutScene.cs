using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class WinCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static WinCutScene WinInstance;

    public bool WinScene = false;

    public GameObject WinCut1;
    public GameObject WinCut2;
    public GameObject WinCut3;
    public GameObject WinCut4;

    int screen = 0;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "WinScreen")
        {
            WinScene = true;
            WinCut1.SetActive(true);
            screen = 0;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (WinInstance == null)
        {
            WinInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (WinScene == true)
        {
            GameObject.Find("Audio Source").GetComponent<AudioSource>().mute = true;
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
        else
        {
            GameObject.Find("Audio Source").GetComponent<AudioSource>().mute = false;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (screen == 0)
            {
                Next();
                screen = 1;
            }
            else if (screen == 1)
            {
                Next1();
                screen = 2;
            }
            else if (screen == 2)
            {
                Next2();
                screen = 3;
            }
            else if (screen == 3)
            {
                Next3();
                screen = 0;
            }
        }
    }

    public void Next()
    {
        WinCut1.SetActive(false);
        WinCut2.SetActive(true);
    }
    public void Next1()
    {
        WinCut2.SetActive(false);
        WinCut3.SetActive(true);
    }
    public void Next2()
    {
        WinCut3.SetActive(false);
        WinCut4.SetActive(true);
    }
    public void Next3()
    {
        WinCut4.SetActive(false);
        WinScene = false;
        SceneManager.LoadScene("MainMenu");
    }
}
