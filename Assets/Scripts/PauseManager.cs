using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        // Toggle pause when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freeze game
        isPaused = true;
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Unfreeze game
        isPaused = false;
        Debug.Log("Game Resumed");
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Always reset time before changing scenes
        SceneManager.LoadScene("Main Menu"); // Replace with your main menu scene name
        Debug.Log("Exiting to Main Menu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quitting Game");
        Application.Quit();

        // For testing in editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
