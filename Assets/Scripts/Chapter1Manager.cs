using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chapter1Manager : MonoBehaviour
{
    public CanvasGroup titleGroup;
    public GameObject dialoguePanel;
    public float introDuration = 2f;
    public float fadeDuration = 2f;

    public DialogueManager dialogueManager;
    public StatsUIUpdater statsUIUpdater;
    public BudgetPanelManager budgetPanelManager;

    public GameObject cutscenePanel;
    public GameObject cutscene1Panel;
    public GameObject budgetPanel;
    public GameObject reportPanel;

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.Instance.currentChapter = 1;

        int budgetPerLevel = (GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo) ? 24000 : 15000;
        int startingBudget = budgetPerLevel + GameManager.Instance.previousLevelRemainingCash;
        GameManager.Instance.SetBudget(startingBudget);

        Debug.Log($"[Level Start] Residence: {GameManager.Instance.chosenResidence}, New Allowance: {budgetPerLevel}, Carried over: {GameManager.Instance.previousLevelRemainingCash}, Starting Budget: {startingBudget}");

        // Initialize all slider min/max/values to match this new budget
        budgetPanelManager.InitializeSlidersToBudget();

        // 5. Optionally update stats UI if not done by BudgetPanelManager
        statsUIUpdater.UpdateUI();

        // 6. Hide dialogue for now, then start intro sequence
        dialoguePanel.SetActive(false);
        StartCoroutine(ShowIntroThenDialogue());
    }

    IEnumerator ShowIntroThenDialogue()
    {
        yield return StartCoroutine(Fade(titleGroup, 0, 1, fadeDuration)); // Fade in
        yield return new WaitForSeconds(introDuration); // Stay
        yield return StartCoroutine(Fade(titleGroup, 1, 0, fadeDuration)); // Fade out
        titleGroup.gameObject.SetActive(false);

        cutscene1Panel.SetActive(true);
        yield return new WaitForSeconds(3f);
        cutscene1Panel.SetActive(false);

        dialoguePanel.SetActive(true); // Show dialogue
        Debug.Log("Starting Dialogue");

        dialogueManager.BeginChapter1Intro();
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

    // Called at end of intro dialogue
    public void ShowBudgetPanel()
    {
        statsUIUpdater.UpdateUI();
        budgetPanel.SetActive(true);
    }
    public void ProceedToCutscene()
    {
        cutscenePanel.SetActive(true);
        StartCoroutine(ShowCutsceneThenDialogue()); 
    }
    IEnumerator ShowCutsceneThenDialogue()
    {
        yield return new WaitForSeconds(2f); // Cutscene duration
        cutscenePanel.SetActive(false);

        // Trigger dialogue
        dialogueManager.BeginBoyetDialogue();
    }

    public void LevelEndReport()
    {
        reportPanel.SetActive(true);
        Debug.Log("Level complete. Showing post-level report.");
    }
}
