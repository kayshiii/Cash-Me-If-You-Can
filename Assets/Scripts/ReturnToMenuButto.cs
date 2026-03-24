using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    // Set this in the Inspector to match your main menu scene name
    [SerializeField] private string mainMenuSceneName = "Main Menu";

    public void OnReturnToMenuClicked()
    {
        // Reset all stats in your GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetStats();  // uses your existing ResetStats method
            GameManager.Instance.chosenResidence = GameManager.ResidenceType.None;
            GameManager.Instance.previousLevelRemainingCash = 0;
            GameManager.Instance.dailyNeedsSpent = 0;
        }

        Time.timeScale = 1f; // just in case the game was paused or slowed

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
