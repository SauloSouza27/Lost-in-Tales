using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    void Update()
    {
        
    }

    public void Resume () {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause () {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OnMenuButton () {
        SceneManager.LoadScene(0);
    }

    public void OnExitButton () {
        Application.Quit();
    }

    public void OnRetryButton () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNextLevelButton () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
