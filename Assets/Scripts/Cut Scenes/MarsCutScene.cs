using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarsCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static MarsCutScene MarsInstance;

    private bool MarsScene = false;

    public GameObject MarsCut1;
    public GameObject MarsCut2;
    public GameObject MarsCut3;

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
