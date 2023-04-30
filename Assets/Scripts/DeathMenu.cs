using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    void Awake()
    {
        Cursor.visible = true;
        //FindObjectOfType<CameraController>().ToggleCursor();
        PlayerController.CutSceneActive = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("HubLevel");
        Cursor.visible = false;
        //FindObjectOfType<CameraController>().ToggleCursor();
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
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
