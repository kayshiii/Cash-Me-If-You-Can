using System.Collections;
using UnityEngine;

public class Chapter6Manager : MonoBehaviour
{
    [Header("Intro")]
    public CanvasGroup titleGroup;
    public float introFadeDuration = 1f;
    public float introStayDuration = 1f;

    [Header("UI References")]
    public GameObject bgImg;
    public GameObject lolaBG;
    public GameObject dialoguePanel;

    [Header("Panels")]
    public GameObject budgetPanel;
    public GameObject cutsceneIntroPanel;   // first cutscene
    public GameObject cutsceneMidPanel;     // second cutscene
    public GameObject reportPanel;

    [Header("Cutscene Next Button")]
    public GameObject cutsceneNextButton;   // reused for both cutscenes

    [Header("Scripts")]
    public DialogueManager dialogueManager;
    public StatsUIUpdater statsUIUpdater;
    public BudgetPanelManager budgetPanelManager;

    [Header("Dialogue Sequences")]
    public DialogueLine[] chapter6IntroLines;   // intro dialogue
    public DialogueLine[] chapter6MidLines;     // mid dialogue (after budget)
    public DialogueLine[] chapter6FinalLines;   // final dialogue (after 2nd cutscene)

    private bool nextCutsceneStep = false;

    public void OnCutsceneNextButton()
    {
        nextCutsceneStep = true;
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.Instance.currentChapter = 6;

        int budgetPerLevel = (GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo) ? 24000 : 15000;
        int startingBudget = budgetPerLevel + GameManager.Instance.previousLevelRemainingCash;
        GameManager.Instance.SetBudget(startingBudget);

        Debug.Log($"[Level Start] Chapter 6, Budget: {startingBudget}");

        budgetPanelManager.InitializeSlidersToBudget();
        statsUIUpdater.UpdateUI();

        StartCoroutine(MainFlow());
    }

    IEnumerator MainFlow()
    {
        // Optional title fade
        if (titleGroup != null)
        {
            yield return StartCoroutine(Fade(titleGroup, 0f, 1f, introFadeDuration));
            yield return new WaitForSeconds(introStayDuration);
            yield return StartCoroutine(Fade(titleGroup, 1f, 0f, introFadeDuration));
            titleGroup.gameObject.SetActive(false);
        }

        // 1) First cutscene
        cutsceneIntroPanel.SetActive(true);
        if (cutsceneNextButton != null) cutsceneNextButton.SetActive(true);
        nextCutsceneStep = false;
        yield return new WaitUntil(() => nextCutsceneStep);
        if (cutsceneNextButton != null) cutsceneNextButton.SetActive(false);
        cutsceneIntroPanel.SetActive(false);

        // 2) Intro dialogue
        bgImg.SetActive(true);
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(chapter6IntroLines));
        dialoguePanel.SetActive(false);

        // 3) Budget screen
        ShowBudgetPanel();
        // Wait until BudgetPanelManager calls ProceedAfterBudget6()
    }

    void ShowBudgetPanel()
    {
        statsUIUpdater.UpdateUI();
        budgetPanel.SetActive(true);
    }

    // Called by BudgetPanelManager when Chapter 6 budget is confirmed
    public void ProceedAfterBudget6()
    {
        StartCoroutine(AfterBudgetFlow());
    }

    IEnumerator AfterBudgetFlow()
    {
        budgetPanel.SetActive(false);

        // 4) Mid dialogue
        dialoguePanel.SetActive(true);
        bgImg.SetActive(false);
        lolaBG.SetActive(true);

        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(chapter6MidLines));

        lolaBG.SetActive(false);
        dialoguePanel.SetActive(false);
        bgImg.SetActive(true);

        // 5) Second cutscene
        cutsceneMidPanel.SetActive(true);
        if (cutsceneNextButton != null) cutsceneNextButton.SetActive(true);
        nextCutsceneStep = false;
        yield return new WaitUntil(() => nextCutsceneStep);
        if (cutsceneNextButton != null) cutsceneNextButton.SetActive(false);
        cutsceneMidPanel.SetActive(false);

        // 6) Final dialogue
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(chapter6FinalLines));
        dialoguePanel.SetActive(false);

        // 7) Report
        LevelEndReport();
    }

    public void LevelEndReport()
    {
        reportPanel.SetActive(true);
        Debug.Log("Chapter 6 complete. Showing post-level report.");
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0f;
        cg.gameObject.SetActive(true);
        cg.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        cg.alpha = to;
        if (Mathf.Approximately(to, 0f))
            cg.gameObject.SetActive(false);
    }
}
