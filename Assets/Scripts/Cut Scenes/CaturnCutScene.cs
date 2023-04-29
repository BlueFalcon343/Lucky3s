using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CaturnCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static CaturnCutScene CaturnInstance;

    public bool CaturnScene = false;

    public GameObject CaturnCut1;
    public GameObject CaturnCut2;
    public GameObject CaturnCut3;
    int screen = 0;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "CaturnLevel")
        {
            CaturnScene = true;
            CaturnCut1.SetActive(true);
            PlayerController.CutSceneActive = true;
        }
    }

    void Awake()
    {
        Cursor.visible = true;
        DontDestroyOnLoad(this.gameObject);

        if (CaturnInstance == null)
        {
            CaturnInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (CaturnScene == true)
        {
            GameObject.Find("Audio Source").GetComponent<AudioSource>().mute = true;
            Time.timeScale = 0f;
        }
        else
        {
            GameObject.Find("Audio Source").GetComponent<AudioSource>().mute = false;
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
        }
    }

    public void Next()
    {
        CaturnCut1.SetActive(false);
        CaturnCut2.SetActive(true);
    }
    public void Next1()
    {
        CaturnCut2.SetActive(false);
        CaturnCut3.SetActive(true);
    }
    public void Next2()
    {
        GameObject.Find("CutsceneMusicCaturn").GetComponent<AudioSource>().mute = true;
        CaturnCut3.SetActive(false);
        CaturnScene = false;
        Cursor.visible = false;
        PlayerController.CutSceneActive = false;
    }
}
