using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenericLevelManager : MonoBehaviour
{
    [Header("Intro")]
    public CanvasGroup titleGroup;      // optional fade-in title
    public float introFadeDuration = 1f;
    public float introStayDuration = 1f;
    private bool nextCutsceneStep = false;

    [Header("UI References")]
    public GameObject bgImg;
    public GameObject dialoguePanel;

    [Header("Notification Popup")]
    public GameObject notifPanel;       // like your popupPanel
    public GameObject notifIcon;        // optional icon
    public TMPro.TextMeshProUGUI notifText;
    public float notifDuration = 2f;

    [Header("Budget / Cutscene / Report")]
    public GameObject budgetPanel;
    public GameObject cutscenePanel;
    public GameObject cutscene1Panel;
    public GameObject reportPanel;
    public GameObject cutsceneNextButton;

    [Header("Scripts")]
    public DialogueManager dialogueManager;
    public StatsUIUpdater statsUIUpdater;
    public BudgetPanelManager budgetPanelManager;

    [Header("Dialogue Sequences")]
    public DialogueLine[] introDialogueLines;    // normal intro (no chapter)
    public DialogueLine[] postCutsceneDialogue;  // dialogue after cutscene

    public void OnCutsceneNextButton()
    {
        nextCutsceneStep = true;
    }

    void Start()
    {
        StartCoroutine(MainFlow());
    }

    IEnumerator MainFlow()
    {
        // 1. Optional title fade
        if (titleGroup != null)
        {
            yield return StartCoroutine(Fade(titleGroup, 0, 1, introFadeDuration));
            yield return new WaitForSeconds(introStayDuration);
            yield return StartCoroutine(Fade(titleGroup, 1, 0, introFadeDuration));
            titleGroup.gameObject.SetActive(false);
        }
        cutscene1Panel.SetActive(true);
        nextCutsceneStep = false;

        // Wait until the player presses the Next button
        yield return new WaitUntil(() => nextCutsceneStep);

        cutscene1Panel.SetActive(false);

        // 2. Intro dialogue
        bgImg.SetActive(true);
        dialoguePanel.SetActive(true);

        // Play only the lines you pass, without touching chapter state
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(introDialogueLines));

        dialoguePanel.SetActive(false);

        // 3. Notification popup
        //yield return StartCoroutine(ShowNotificationPopup());

        // 4. Budget screen
        ShowBudgetScreen();
        // Here you let the BudgetPanelManager drive when to proceed.
        // When finished, it should call ProceedToCutscene() on this manager.
    }

    /*IEnumerator ShowNotificationPopup()
    {
        if (notifPanel == null || notifText == null)
            yield break;

        notifPanel.SetActive(true);
        if (notifIcon != null) notifIcon.SetActive(true);

        // notifText.text should already be set in Inspector, or set it here:
        // notifText.text = "+ PHP 5,000 (Bonus)";

        yield return new WaitForSeconds(notifDuration);

        notifPanel.SetActive(false);
        if (notifIcon != null) notifIcon.SetActive(false);
    }*/

    void ShowBudgetScreen()
    {
        statsUIUpdater.UpdateUI();
        budgetPanel.SetActive(true);
    }

    // Called by BudgetPanelManager when player confirms budget
    public void ProceedToCutscene()
    {
        StartCoroutine(CutsceneThenDialogueThenReport());
    }

    IEnumerator CutsceneThenDialogueThenReport()
    {
        // 5. Cutscene (with your existing Next button pattern if desired)
        cutscenePanel.SetActive(true);
        if (cutsceneNextButton != null)
            cutsceneNextButton.SetActive(true);

        nextCutsceneStep = false;
        yield return new WaitUntil(() => nextCutsceneStep);

        if (cutsceneNextButton != null)
            cutsceneNextButton.SetActive(false);
        cutscenePanel.SetActive(false);

        // 6. Dialogue after cutscene
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(dialogueManager.PlayDialogueSequence(postCutsceneDialogue));
        dialoguePanel.SetActive(false);

        // 7. Report
        reportPanel.SetActive(true);
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
