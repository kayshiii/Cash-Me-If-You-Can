using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class InfoLine
{
    [TextArea(2, 5)]
    public string text;
}

public class TutorialBudgetAlloc : MonoBehaviour
{
    public GameObject sliderGameObj;
    public GameObject cashOnHand;
    public GameObject cashOnHandInfo;
    public GameObject iponSlider;
    public GameObject iponBubble;
    public GameObject dailyNeedsSlider;
    public GameObject dailyNeedsBubble;
    public GameObject lakwatsaSlider;
    public GameObject lakwatsaBubble;
    public GameObject confirmButton;

    public GameObject promptGameObj;
    public GameObject values;
    public GameObject social;
    public GameObject socialInfo;
    public GameObject happiness;
    public GameObject happinessInfo;
    public GameObject focus;
    public GameObject focusInfo;
    public GameObject budgetGoal;
    public GameObject budgetGoalInfo;
    public GameObject currentCash;
    public GameObject currentCashInfo;

    public GameObject nextInfoIcon;

    public GameObject choicesObject;
    public GameObject staycationAnim;
    public GameObject savingAnim;
    public GameObject sellingAnim;

    public GameObject choices;
    public GameObject uwianAnim;
    public GameObject condoAnim;

    public DialogueManager dialogueManager;
    public GameManager gameManager;

    private bool skipTypewriter = false;
    private bool isTypingInfo = false;
    private bool proceedToNextStep = false;

    public float delayBetweenReveals = 3.5f;

    // UI text fields
    public TextMeshProUGUI OnhandcashText;
    public TextMeshProUGUI iponInfoText;
    public TextMeshProUGUI dailyNeedsInfoText;
    public TextMeshProUGUI lakwatsaInfoText;

    public TextMeshProUGUI promptText;

    public TextMeshProUGUI socialText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI focusText;
    public TextMeshProUGUI budgetGoalText;
    public TextMeshProUGUI currentCashText;

    // Data (Inspector-editable, like DialogueLine)
    public InfoLine onhandCashInfo;
    public InfoLine iponInfo;
    public InfoLine dailyNeedsInfo;
    public InfoLine lakwatsaInfo;

    public InfoLine promptMessage;
    public InfoLine finalPromptMessage;

    public InfoLine socialInfoText;
    public InfoLine happinessInfoText;
    public InfoLine focusInfoText;
    public InfoLine budgetGoalInfoText;
    public InfoLine currentCashInfoText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTypingInfo)
            {
                // First press while typing → reveal instantly
                skipTypewriter = true;
            }
            else
            {
                // Second press after text is fully shown → go to next step
                proceedToNextStep = true;
            }
        }
    }

    public void StartBudgetTutorial()
    {
        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        // Give initial budget to player
        GameManager.Instance.AddBudget(5000);

        sliderGameObj.SetActive(true);

        // Show Cash On Hand
        cashOnHand.SetActive(true);
        cashOnHandInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(OnhandcashText, onhandCashInfo.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show Ipon slider & info
        iponSlider.SetActive(true);
        iponBubble.SetActive(true);
        yield return StartCoroutine(TypeInfo(iponInfoText, iponInfo.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show Daily Needs slider & info
        dailyNeedsSlider.SetActive(true);
        dailyNeedsBubble.SetActive(true);
        yield return StartCoroutine(TypeInfo(dailyNeedsInfoText, dailyNeedsInfo.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show Lakwatsa slider & info
        lakwatsaSlider.SetActive(true);
        lakwatsaBubble.SetActive(true);
        yield return StartCoroutine(TypeInfo(lakwatsaInfoText, lakwatsaInfo.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        confirmButton.SetActive(true);

        // prompt to proceed
        /*sliderGameObj.SetActive(false);
        promptGameObj.SetActive(true);*/
        StartCoroutine(RevealValues());
    }

    IEnumerator TypeInfo(TextMeshProUGUI infoText, string infoString)
    {
        isTypingInfo = true;
        skipTypewriter = false;
        proceedToNextStep = false;
        infoText.text = "";

        if (nextInfoIcon != null) nextInfoIcon.SetActive(false);

        foreach (char c in infoString)
        {
            if (skipTypewriter)
            {
                infoText.text = infoString;
                break;
            }

            infoText.text += c;
            yield return new WaitForSeconds(0.01f);
        }

        isTypingInfo = false;

        // show “next” cue
        if (nextInfoIcon != null) nextInfoIcon.SetActive(true);

        // wait for Space / click to move on
        yield return new WaitUntil(() => proceedToNextStep);
        proceedToNextStep = false;

        // hide again after the press
        if (nextInfoIcon != null) nextInfoIcon.SetActive(false);
    }

    IEnumerator RevealValues()
    {
        values.SetActive(true);

        // Show social value & info
        social.SetActive(true);
        socialInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(socialText, socialInfoText.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show happiness value & info
        happiness.SetActive(true);
        happinessInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(happinessText, happinessInfoText.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show focus value & info
        focus.SetActive(true);
        focusInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(focusText, focusInfoText.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show budget goal value & info
        budgetGoal.SetActive(true);
        budgetGoalInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(budgetGoalText, budgetGoalInfoText.text));
        //yield return new WaitForSeconds(delayBetweenReveals);

        // Show current cash value & info
        currentCash.SetActive(true);
        currentCashInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(currentCashText, currentCashInfoText.text));
        //yield return new WaitForSeconds(delayBetweenReveals);
        currentCashInfo.SetActive(false);

        values.SetActive(false);
        sliderGameObj.SetActive(false);
        promptGameObj.SetActive(true);
        yield return StartCoroutine(TypeInfo(promptText, promptMessage.text));
        //yield return new WaitForSeconds(3f);

        promptGameObj.SetActive(true);
        choicesObject.SetActive(true);
    }

    public void OnLakwatsaChoice()
    {
        GameManager.Instance.AddBudget(-2000);
        GameManager.Instance.AddHappiness(10);
        GameManager.Instance.AddSocial(10);

        GameManager.Instance.previousLevelRemainingCash = GameManager.Instance.budget;

        choicesObject.SetActive(false);
        StartCoroutine(StaycationAftermath());
    }

    public void OnSaveChoice()
    {
        GameManager.Instance.AddIpon(1000);
        GameManager.Instance.AddSocial(-10);

        GameManager.Instance.previousLevelRemainingCash = GameManager.Instance.budget;

        choicesObject.SetActive(false);
        StartCoroutine(SavingAftermath());
    }

    public void OnSellChoice()
    {
        GameManager.Instance.AddBudget(2000);
        GameManager.Instance.AddHappiness(5);

        GameManager.Instance.previousLevelRemainingCash = GameManager.Instance.budget;

        choicesObject.SetActive(false);
        StartCoroutine(SellingAftermath());
    }

    IEnumerator StaycationAftermath()
    {
        staycationAnim.SetActive(true);
        yield return new WaitForSeconds(2f);

        staycationAnim.SetActive(false);
        values.SetActive(false);
        promptGameObj.SetActive(false);
        dialogueManager.BeginFinalTutorialDialogue();
    }

    IEnumerator SavingAftermath()
    {
        savingAnim.SetActive(true);
        Debug.Log("Playing saving aftermath animation");
        yield return new WaitForSeconds(2f);
        Debug.Log("done saving aftermath animation");

        savingAnim.SetActive(false);
        values.SetActive(false);
        promptGameObj.SetActive(false);
        dialogueManager.BeginFinalTutorialDialogue();
    }

    IEnumerator SellingAftermath()
    {
        sellingAnim.SetActive(true);
        yield return new WaitForSeconds(2f);

        sellingAnim.SetActive(false);
        values.SetActive(false);
        promptGameObj.SetActive(false);
        dialogueManager.BeginFinalTutorialDialogue();
    }

    public void StartFinalPrompt()
    {
        StartCoroutine(ShowPromptFinal());
    }

    IEnumerator ShowPromptFinal()
    {
        promptGameObj.SetActive(true);
        yield return StartCoroutine(TypeInfo(promptText, finalPromptMessage.text));
        yield return new WaitForSeconds(4f);

        choices.SetActive(true);
    }

    public void OnUwianChoice()
    {
        choices.SetActive(false);
        promptGameObj.SetActive(false);
        GameManager.Instance.chosenResidence = GameManager.ResidenceType.Uwian;
        Debug.Log("Uwian chosen, budget set to 15000");

        StartCoroutine(UwianTransition());
    }

    public void OnCondoChoice()
    {
        choices.SetActive(false);
        promptGameObj.SetActive(false);
        GameManager.Instance.chosenResidence = GameManager.ResidenceType.Condo;
        Debug.Log("Condo chosen, budget set to 24000");

        StartCoroutine(CondoTransition());
    }

    IEnumerator UwianTransition()
    {
        uwianAnim.SetActive(true);
        yield return new WaitForSeconds(2f);

        uwianAnim.SetActive(false);
        SceneManager.LoadScene("Chapter 1");
    }

    IEnumerator CondoTransition()
    {
        condoAnim.SetActive(true);
        yield return new WaitForSeconds(2f);

        condoAnim.SetActive(false);
        SceneManager.LoadScene("Chapter 1");
    }
}
