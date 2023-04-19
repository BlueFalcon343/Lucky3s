using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class JupiterCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static JupiterCutScene JupiterInstance;

    public bool JupiterScene = false;

    public GameObject JupiterCut1;
    public GameObject JupiterCut2;
    public GameObject JupiterCut3;
    int screen = 0;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "JupiterLevel")
        {
            JupiterScene = true;
            JupiterCut1.SetActive(true);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (JupiterInstance == null)
        {
            JupiterInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }



    }

    void Update()
    {
        if (JupiterScene == true)
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
        }
    }

    public void Next()
    {
        JupiterCut1.SetActive(false);
        JupiterCut2.SetActive(true);

    }
    public void Next1()
    {
        JupiterCut2.SetActive(false);
        JupiterCut3.SetActive(true);
    }
    public void Next2()
    {
        JupiterCut3.SetActive(false);
        JupiterScene = false;
    }
}
