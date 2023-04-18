using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static WinCutScene WinInstance;

    private bool WinScene = false;

    public GameObject WinCut1;
    public GameObject WinCut2;
    public GameObject WinCut3;
    public GameObject WinCut4;

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "WinScreen")
        {
            WinScene = true;
            WinCut1.SetActive(true);
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
    }
}
