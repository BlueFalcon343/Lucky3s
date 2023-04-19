using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MarsCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static MarsCutScene MarsInstance;

    public bool MarsScene = false;

    public GameObject MarsCut1;
    public GameObject MarsCut2;
    public GameObject MarsCut3;
    int screen = 0;

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "MarsLevel")
        {
            MarsScene = true;
            MarsCut1.SetActive(true);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (MarsInstance == null)
        {
            MarsInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (MarsScene == true)
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
                screen = 0;
            }
        }
    }

    public void Next()
    {
        MarsCut1.SetActive(false);
        MarsCut2.SetActive(true);
    }
    public void Next1()
    {
        MarsCut2.SetActive(false);
        MarsCut3.SetActive(true);
    }
    public void Next2()
    {
        MarsCut3.SetActive(false);
        MarsScene = false;
    }
}
