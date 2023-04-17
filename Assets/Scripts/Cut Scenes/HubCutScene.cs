using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HubCutScene : MonoBehaviour
{

    public GameObject CutSceneUI;

    private static HubCutScene HubInstance;

    private bool HubScene = false;

    public GameObject IntroCut1;
    public GameObject IntroCut2;
    public GameObject IntroCut3;
    public GameObject IntroCut4;

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "HubLevel")
        {
            HubScene = true;
            IntroCut1.SetActive(true);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (HubInstance == null)
        {
            HubInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }



    }

    void Update()
    {
        if (HubScene == true)
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
        IntroCut1.SetActive(false);
        IntroCut2.SetActive(true);
    }
    public void Next1()
    {
        IntroCut2.SetActive(false);
        IntroCut3.SetActive(true);
    }
    public void Next2()
    {
        IntroCut3.SetActive(false);
        IntroCut4.SetActive(true);
    }
    public void Next3()
    {
        IntroCut4.SetActive(false);
        HubScene = false;
    }
}

