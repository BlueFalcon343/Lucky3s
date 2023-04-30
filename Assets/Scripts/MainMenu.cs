using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject credits;
    public GameObject controls;

    int screen = 0;

    void Start()
    {

    }
    
    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("HubLevel");
        PlayerController.CutSceneActive = false;
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    void OnCredits()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
        screen = 1;
    }
    void OnControls()
    {
        controls.SetActive(true);
        mainMenu.SetActive(false);
        screen = 1;
    }
    void OffCredits()
    {
        mainMenu.SetActive(true);
        credits.SetActive(false);
        screen = 0;
    }
    void OffControls()
    {
        mainMenu.SetActive(true);
        controls.SetActive(false);
        screen = 0;
    }

    public void OnY()
    {
        if (screen == 0)
        {
            screen = 3;
            PlayGame();
        }
    }
    public void OnX()
    {
        if (screen == 0)
        {
            OnCredits();
        }
    }
    public void OnA()
    {
        if (screen == 0)
        {
            QuitGame();
        }
    }
    public void OnB()
    {
        if (screen == 0)
        {
            OnControls();
        }
        else if (screen == 1)
        {
            OffCredits();
            OffControls();
        }
    }

    void Check()
    {
        Debug.Log(screen);
    }
}