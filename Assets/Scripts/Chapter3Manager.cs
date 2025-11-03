using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.HID.HID;
using static UnityEngine.ParticleSystem;

public class Chapter3Manager : MonoBehaviour
{
    public CanvasGroup titleGroup;
    public GameObject bgImg;
    public GameObject dialoguePanel;
    public float introDuration = 2f;
    public float fadeDuration = 2f;

    public DialogueManager dialogueManager;
    public StatsUIUpdater statsUIUpdater;
    public BudgetPanelManager budgetPanelManager;

    public GameObject budgetPanel;
    public GameObject button1;
    public GameObject button2;
    public GameObject spendingPanel;
    public GameObject reportPanel;
    public GameObject cutscenePanel;
    public GameObject cutscene1Panel;
    public GameObject phonePanel;
    public GameObject lolaDecisionPanel;
    public GameObject choicesPanel;

    public List<GameObject> phoneChatBubbles; // Assign in order Boyet/Alex
    public int lakwatsaEventMin = 2000;

    // Temporary storage for first budget values
    private int tempIpon;
    private int tempDailyNeeds;
    private int tempLakwatsa;
    private int initialBudgetBeforeDecision;

    private bool skipTypewriter = false;
    private bool isTypingInfo = false;

    // prompt text
    public TextMeshProUGUI promptText;
    public string promptMessage = "What do you think Alex? This could be a good time to bond with your classmates. And who knows? This might even lead to new opportunities in the future! " +
        "Pero, it could also be better to just save some money for your Ipon.It also never hurts to be more prepared for any unfortunate situation that could happen!";

    void Start()
    {
        Time.timeScale = 1f; // Reset time
        GameManager.Instance.currentChapter = 3;

        int budgetPerLevel = (GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo) ? 24000 : 15000;
        int startingBudget = budgetPerLevel + GameManager.Instance.previousLevelRemainingCash;
        GameManager.Instance.SetBudget(startingBudget);

        Debug.Log($"[Level Start] Chapter 3, Budget: {startingBudget}");

        budgetPanelManager.InitializeSlidersToBudget();
        statsUIUpdater.UpdateUI();

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

        bgImg.SetActive(true);
        titleGroup.gameObject.SetActive(false);
        dialoguePanel.SetActive(true);

        dialogueManager.BeginChapter3Intro();
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
        dialoguePanel.SetActive(false);
        statsUIUpdater.UpdateUI();
        budgetPanel.SetActive(true);
    }

    public void StartPhoneSeq()
    {
        StartCoroutine(ShowPhoneSequence());
    }

    IEnumerator ShowPhoneSequence()
    {
        // 3. Lola: "Mukhang nagtext kaibigan mo"
        phonePanel.SetActive(true);
        yield return new WaitForSeconds(2f);

        // 4. Pop up each bubble for phone chat with a short delay
        foreach (var bubble in phoneChatBubbles)
        {
            bubble.SetActive(true);
            yield return new WaitForSeconds(2f); // or tap/click to continue
        }

        // 5. Decision prompt after last chat
        ShowLolaDecision();
    }

    public void ConfirmFirstBudgetFromSliders()
    {
        int ipon = Mathf.RoundToInt(budgetPanelManager.iponSlider.value);
        int dailyNeeds = Mathf.RoundToInt(budgetPanelManager.dailyNeedsSlider.value);
        int lakwatsa = Mathf.RoundToInt(budgetPanelManager.lakwatsaSlider.value);

        ConfirmFirstBudget(ipon, dailyNeeds, lakwatsa);
    }

    public void ConfirmFirstBudget(int ipon, int dailyNeeds, int lakwatsa)
    {
        // Store the first confirmed budget values temporarily
        tempIpon = ipon;
        tempDailyNeeds = dailyNeeds;
        tempLakwatsa = lakwatsa;
        int spentTotal = tempIpon + tempDailyNeeds + tempLakwatsa;

        int startingBudget = GameManager.Instance.budget;
        int cashRemaining = startingBudget - spentTotal;

        // Store initial budget before decision to restore if declined
        initialBudgetBeforeDecision = GameManager.Instance.budget;

        Debug.Log($"[CONFIRM] Saved: Ipon={ipon}, DailyNeeds={dailyNeeds}, Lakwatsa={lakwatsa}, Remaining={cashRemaining}");

        budgetPanel.SetActive(false);

        dialoguePanel.SetActive(true);
        dialogueManager.BeginChapter3Lola();
    }

    public void promptTextFunc()
    {
        StartCoroutine(promptTexting());
    }

    IEnumerator promptTexting()
    {
        yield return StartCoroutine(TypeInfo(promptText, promptMessage));
        yield return new WaitForSeconds(4f); // or require spacebar to continue
    }
    IEnumerator TypeInfo(TextMeshProUGUI infoText, string infoString)
    {
        isTypingInfo = true;
        skipTypewriter = false;
        infoText.text = "";

        foreach (char c in infoString)
        {
            if (skipTypewriter)
            {
                infoText.text = infoString;
                break;
            }
            infoText.text += c;
            yield return new WaitForSeconds(0.035f); // adjust as needed
        }

        isTypingInfo = false;
    }

    public void ShowLolaDecision()
    {
        phonePanel.SetActive(false);
        lolaDecisionPanel.SetActive(true);

        promptTextFunc();

        choicesPanel.SetActive(true);
    }

    public void OnDecisionTripAccept()
    {
        choicesPanel.SetActive(false);
        lolaDecisionPanel.SetActive(false);

        // Reset the budget back to initial before the decision
        GameManager.Instance.SetBudget(initialBudgetBeforeDecision);

        // Show budget panel for re-allocation (second time)
        budgetPanel.SetActive(true);
        budgetPanelManager.ResetSlidersForReallocation(true, lakwatsaEventMin);
        button1.SetActive(false);
        button2.SetActive(true);

        // Re-initialize ALL sliders from scratch with reset budget
        budgetPanelManager.iponSlider.value = 0; // Or any preferred default
        budgetPanelManager.dailyNeedsSlider.value = 0;
        budgetPanelManager.lakwatsaSlider.minValue = lakwatsaEventMin;
        budgetPanelManager.lakwatsaSlider.value = lakwatsaEventMin; // Force minimum for lakwatsa
    }
    public void OnSecondBudgetConfirmed()
    {
        budgetPanel.SetActive(false);
        // Stat effect for joining: +15 happiness, +10 social, -5 focus
        GameManager.Instance.AddHappiness(15);
        GameManager.Instance.AddSocial(10);
        GameManager.Instance.AddFocus(-5);

        // finalize budget values
        GameManager.Instance.SetIpon(Mathf.RoundToInt(budgetPanelManager.iponSlider.value));
        GameManager.Instance.dailyNeedsSpent = Mathf.RoundToInt(budgetPanelManager.dailyNeedsSlider.value);
        GameManager.Instance.SetLakwatsa(Mathf.RoundToInt(budgetPanelManager.lakwatsaSlider.value));
        GameManager.Instance.SetBudget(GameManager.Instance.budget - (GameManager.Instance.ipon + GameManager.Instance.dailyNeedsSpent + GameManager.Instance.lakwatsa));

        ProceedToCutscene();
    }

    public void OnDecisionTripDecline()
    {
        lolaDecisionPanel.SetActive(false);

        // Use stored temp values as final values for the game state
        GameManager.Instance.SetIpon(tempIpon);
        GameManager.Instance.dailyNeedsSpent = tempDailyNeeds;
        GameManager.Instance.SetLakwatsa(tempLakwatsa);

        // Recalculate budget based on total initial budget minus allocated amounts
        GameManager.Instance.SetBudget(initialBudgetBeforeDecision - (tempIpon + tempDailyNeeds + tempLakwatsa));

        // Apply stat effects for declining the invitation
        GameManager.Instance.AddFocus(10);
        GameManager.Instance.AddHappiness(-8);
        GameManager.Instance.AddSocial(-8);

        // Proceed to the cutscene and report flow
        ProceedToCutscene();
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
        Debug.Log("Chapter 3 complete. Showing post-level report.");
    }
}
