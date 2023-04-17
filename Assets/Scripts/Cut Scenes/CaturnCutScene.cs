using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CaturnCutScene : MonoBehaviour
{
    public GameObject CutSceneUI;

    private static CaturnCutScene CaturnInstance;

    private bool CaturnScene = false;

    public GameObject CaturnCut1;
    public GameObject CaturnCut2;
    public GameObject CaturnCut3;

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "CaturnLevel")
        {
            CaturnScene = true;
            CaturnCut1.SetActive(true);
        }
    }

    void Awake()
    {
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
        CaturnCut3.SetActive(false);
        CaturnScene = false;
    }
}