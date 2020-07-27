using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Archipelago");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
