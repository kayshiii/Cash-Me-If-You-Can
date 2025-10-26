using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReportManager : MonoBehaviour
{
    public TextMeshProUGUI fundsSavedText;
    public TextMeshProUGUI fundsAllocatedText;

    public Image socialFillImage;
    public Image happinessFillImage;
    public Image focusFillImage;

    public Button continueButton;

    private void OnEnable()
    {
        UpdateReportUI();
    }

    public void UpdateReportUI()
    {
        // Get values from GameManager
        int fundsSaved = GameManager.Instance.budget;
        int fundsAllocated = GameManager.Instance.ipon;
        int social = GameManager.Instance.social;
        int happiness = GameManager.Instance.happiness;
        int focus = GameManager.Instance.focus;

        // Update text values
        fundsSavedText.text = fundsSaved.ToString("N0");
        fundsAllocatedText.text = fundsAllocated.ToString("N0");

        // Update stat circles (assuming max value of 100 for each stat)
        socialFillImage.fillAmount = social / 100f;
        happinessFillImage.fillAmount = happiness / 100f;
        focusFillImage.fillAmount = focus / 100f;
    }

    public void OnContinueClicked()
    {
        Time.timeScale = 1f;
        // Check current chapter and load next
        if (GameManager.Instance.currentChapter == 1)
        {
            GameManager.Instance.currentChapter = 2;
            SceneManager.LoadScene("Chapter 2");
        }
        else if (GameManager.Instance.currentChapter == 2)
        {
            GameManager.Instance.currentChapter = 3;
            SceneManager.LoadScene("Chapter 3"); // when you create it
        }
    }
}
