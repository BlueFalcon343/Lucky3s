using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public void LoadMenu()
    {
        SceneManager.LoadScene("MarsLevel");
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
