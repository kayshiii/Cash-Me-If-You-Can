using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("Intro Screen");
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
