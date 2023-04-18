using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private static PauseMenu PauseInstance;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (PauseInstance == null)
        {
            PauseInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                FindObjectOfType<CameraController>().ToggleCursor();
            }
            else
            {
                Pause();
                FindObjectOfType<CameraController>().ToggleCursor();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        FindObjectOfType<CameraController>().ToggleCursor();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        FindObjectOfType<CameraController>().ToggleCursor();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}
