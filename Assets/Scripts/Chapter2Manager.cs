using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chapter2Manager : MonoBehaviour
{
    public CanvasGroup titleGroup;
    public GameObject dialoguePanel;
    public float introDuration = 2f;
    public float fadeDuration = 2f;

    public DialogueManager dialogueManager;
    public StatsUIUpdater statsUIUpdater;
    public BudgetPanelManager budgetPanelManager;
    public GameObject budgetPanel;
    public GameObject spendingPanel;
    public GameObject reportPanel;
    public GameObject cutscenePanel;
    public GameObject cutscene1Panel;

    void Start()
    {
        Time.timeScale = 1f; // Reset time
        GameManager.Instance.currentChapter = 2;

        int budgetPerLevel = (GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo) ? 24000 : 15000;
        int startingBudget = budgetPerLevel + GameManager.Instance.previousLevelRemainingCash;
        GameManager.Instance.SetBudget(startingBudget);

        Debug.Log($"[Level Start] Chapter 2, Budget: {startingBudget}");

        budgetPanelManager.InitializeSlidersToBudget();
        statsUIUpdater.UpdateUI();

        dialoguePanel.SetActive(false);
        StartCoroutine(ShowIntroThenDialogue());
    }

    IEnumerator ShowIntroThenDialogue()
    {
        yield return StartCoroutine(Fade(titleGroup, 0, 1, fadeDuration));
        yield return new WaitForSeconds(introDuration);
        yield return StartCoroutine(Fade(titleGroup, 1, 0, fadeDuration));

        cutscene1Panel.SetActive(true);
        yield return new WaitForSeconds(3f);
        cutscene1Panel.SetActive(false);

        titleGroup.gameObject.SetActive(false);
        dialoguePanel.SetActive(true);

        dialogueManager.BeginChapter2Intro();
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = to;
    }

    public void ShowBudgetPanel()
    {
        statsUIUpdater.UpdateUI();
        budgetPanel.SetActive(true);
    }

    public void ProceedToCutscene()
    {
        cutscenePanel.SetActive(true);
        StartCoroutine(ShowCutsceneThenReport());
    }

    IEnumerator ShowCutsceneThenReport()
    {
        yield return new WaitForSeconds(3f);
        cutscenePanel.SetActive(false);
        LevelEndReport();
    }

    public void LevelEndReport()
    {
        reportPanel.SetActive(true);
        Debug.Log("Chapter 2 complete. Showing post-level report.");
    }
}
