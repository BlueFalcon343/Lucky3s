using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public void LoadMenu()
    {
        SceneManager.LoadScene("HubLevel");
        FindObjectOfType<CameraController>().ToggleCursor();
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnA()
    {
        LoadMenu();
    }
    public void OnB()
    {
        QuitGame();
    }
}
