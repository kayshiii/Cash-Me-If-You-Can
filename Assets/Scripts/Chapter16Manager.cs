using System.Collections;
using UnityEngine;

public class Chapter16Manager : MonoBehaviour
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
    public GameObject cutsceneIntroPanel;        // first cutscene
    public GameObject cutsceneAfterEventPanel;   // final cutscene
    public GameObject reportPanel;

    [Header("Random Event Prompt")]
    public GameObject randomEventPanel;
    public TMPro.TextMeshProUGUI randomEventText;
    [TextArea(2, 5)]
    public string randomEventMessage;
    public GameObject randomEventButtons;

    [Header("Cutscene Next Button")]
    public GameObject cutsceneNextButton;        // reused for both cutscenes

    [Header("Scripts")]
    public DialogueManager dialogueManager;
    public StatsUIUpdater statsUIUpdater;
    public BudgetPanelManager budgetPanelManager;

    [Header("Dialogue Sequences")]
    public DialogueLine[] chapter16IntroLines;   // intro dialogue
    public DialogueLine[] chapter16MidLines;     // mid dialogue (after budget)
    public DialogueLine[] chapter16FinalLines;   // final dialogue (after random event)

    private bool nextCutsceneStep = false;

    // === Buttons ===
    public void OnCutsceneNextButton()
    {
        nextCutsceneStep = true;
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.Instance.currentChapter = 16;

        int budgetPerLevel = (GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo) ? 24000 : 15000;
        int startingBudget = budgetPerLevel + GameManager.Instance.previousLevelRemainingCash;
        GameManager.Instance.SetBudget(startingBudget);

        Debug.Log($"[Level Start] Chapter 16, Budget: {startingBudget}");

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
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(chapter16IntroLines));
        dialoguePanel.SetActive(false);

        // 3) Budget screen
        ShowBudgetPanel();
        // Wait for BudgetPanelManager to call ProceedAfterBudget16()
    }

    void ShowBudgetPanel()
    {
        statsUIUpdater.UpdateUI();
        budgetPanel.SetActive(true);
    }

    // === Called by BudgetPanelManager when Chapter 16 budget is confirmed ===
    public void ProceedAfterBudget16()
    {
        StartCoroutine(AfterBudgetFlow());
    }

    IEnumerator AfterBudgetFlow()
    {
        budgetPanel.SetActive(false);

        // 4) Mid dialogue
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(chapter16MidLines));
        dialoguePanel.SetActive(false);

        // 5) Random event prompt
        ShowRandomEventPrompt();
        // Flow continues from choice handlers → ContinueAfterRandomEvent()
    }

    void ShowRandomEventPrompt()
    {
        if (randomEventPanel != null)
            randomEventPanel.SetActive(true);

        if (randomEventText != null)
            randomEventText.text = randomEventMessage;

        if (randomEventButtons != null)
            randomEventButtons.SetActive(true);
    }

    // === Random event choices (examples; adjust to your story) ===
    public void OnRandomEventChoiceRepair()
    {
        GameManager.Instance.AddIpon(-5000);
        GameManager.Instance.AddHappiness(5);
        CloseRandomEventAndContinue();
    }

    public void OnRandomEventChoiceBorrow()
    {
        GameManager.Instance.AddSocial(-20);
        CloseRandomEventAndContinue();
    }

    void CloseRandomEventAndContinue()
    {
        if (randomEventButtons != null)
            randomEventButtons.SetActive(false);
        if (randomEventPanel != null)
            randomEventPanel.SetActive(false);

        StartCoroutine(ContinueAfterRandomEvent());
    }

    IEnumerator ContinueAfterRandomEvent()
    {
        // 6) Final dialogue (after random event)
        dialoguePanel.SetActive(true);
        lolaBG.SetActive(true);
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(chapter16FinalLines));
        dialoguePanel.SetActive(false);
        lolaBG.SetActive(false);

        // 7) Final cutscene
        cutsceneAfterEventPanel.SetActive(true);
        if (cutsceneNextButton != null) cutsceneNextButton.SetActive(true);
        nextCutsceneStep = false;
        yield return new WaitUntil(() => nextCutsceneStep);
        if (cutsceneNextButton != null) cutsceneNextButton.SetActive(false);
        cutsceneAfterEventPanel.SetActive(false);

        // 8) Report
        LevelEndReport();
    }

    public void LevelEndReport()
    {
        reportPanel.SetActive(true);
        Debug.Log("Chapter 16 complete. Showing post-level report.");
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
