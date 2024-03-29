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
        FindObjectOfType<PlayerController>().currentHealth = 10;
        SceneManager.LoadScene("HubLevel");
        Cursor.visible = false;
        PlayerController.CutSceneActive = false;
        //FindObjectOfType<CameraController>().ToggleCursor();
    }
    public void QuitGame()
    {
        FindObjectOfType<PlayerController>().currentHealth = 10;
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true; 
        PlayerController.CutSceneActive = true;
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
