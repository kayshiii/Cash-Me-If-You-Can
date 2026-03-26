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

        // 1) ENDING CONDITION HERE
        if (GameManager.Instance.happiness <= 0 ||
            GameManager.Instance.social <= 0 ||
            GameManager.Instance.focus <= 0)
        {
            SceneManager.LoadScene("BadEnding");
            return;
        }

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
        else if (GameManager.Instance.currentChapter == 3)
        {
            GameManager.Instance.currentChapter = 4;
            SceneManager.LoadScene("Chapter 4"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 4)
        {
            GameManager.Instance.currentChapter = 5;
            SceneManager.LoadScene("Chapter 5"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 5)
        {
            GameManager.Instance.currentChapter = 6;
            SceneManager.LoadScene("Chapter 6"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 6)
        {
            GameManager.Instance.currentChapter = 7;
            SceneManager.LoadScene("Chapter 7"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 7)
        {
            GameManager.Instance.currentChapter = 8;
            SceneManager.LoadScene("Chapter 8"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 8)
        {
            GameManager.Instance.currentChapter = 9;
            SceneManager.LoadScene("Chapter 9"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 9)
        {
            GameManager.Instance.currentChapter = 10;
            SceneManager.LoadScene("Chapter 10"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 10)
        {
            GameManager.Instance.currentChapter = 11;
            SceneManager.LoadScene("Chapter 11"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 11)
        {
            GameManager.Instance.currentChapter = 12;
            SceneManager.LoadScene("Chapter 12"); // when you create it
        }
        else if (GameManager.Instance.currentChapter == 12)
        {
            GameManager.Instance.currentChapter = 13;
            SceneManager.LoadScene("Chapter 13");
        }
        else if (GameManager.Instance.currentChapter == 13)
        {
            GameManager.Instance.currentChapter = 14;
            SceneManager.LoadScene("Chapter 14");
        }
        else if (GameManager.Instance.currentChapter == 14)
        {
            GameManager.Instance.currentChapter = 15;
            SceneManager.LoadScene("Chapter 15");
        }
        else if (GameManager.Instance.currentChapter == 15)
        {
            GameManager.Instance.currentChapter = 16;
            SceneManager.LoadScene("Chapter 16");
        }
        else if (GameManager.Instance.currentChapter == 16)
        {
            GameManager.Instance.currentChapter = 17;
            SceneManager.LoadScene("Chapter 17");
        }
        else if (GameManager.Instance.currentChapter == 17)
        {
            GameManager.Instance.currentChapter = 18;
            SceneManager.LoadScene("Chapter 18");
        }
        else if (GameManager.Instance.currentChapter == 18)
        {
            GameManager.Instance.currentChapter = 19;
            SceneManager.LoadScene("Chapter 19");
        }
        else if (GameManager.Instance.currentChapter == 19)
        {
            GameManager.Instance.currentChapter = 20;
            SceneManager.LoadScene("Chapter 20");
        }
        else if (GameManager.Instance.currentChapter == 20)
        {
            GameManager.Instance.currentChapter = 21;
            SceneManager.LoadScene("Chapter 21");
        }
        else if (GameManager.Instance.currentChapter == 21)
        {
            GameManager.Instance.currentChapter = 22;
            SceneManager.LoadScene("Chapter 22");
        }
        else if (GameManager.Instance.currentChapter == 22)
        {
            GameManager.Instance.currentChapter = 23;
            SceneManager.LoadScene("Chapter 23");
        }
        else if (GameManager.Instance.currentChapter == 23)
        {
            GameManager.Instance.currentChapter = 24;
            SceneManager.LoadScene("Chapter 24");
        }
        else if (GameManager.Instance.currentChapter == 24)
        {
            GameManager.Instance.currentChapter = 25;
            SceneManager.LoadScene("Chapter 25");
        }
        else if (GameManager.Instance.currentChapter == 25)
        {
            SceneManager.LoadScene("ToBeContinued");   // your “to be continued” screen
        }
    }
}
