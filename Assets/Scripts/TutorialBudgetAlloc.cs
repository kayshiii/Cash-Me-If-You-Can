using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialBudgetAlloc : MonoBehaviour
{
    public GameObject sliderGameObj;
    public GameObject cashOnHand;
    public GameObject cashOnHandInfo;
    public GameObject iponSlider;
    public GameObject iponInfo;
    public GameObject dailyNeedsSlider;
    public GameObject dailyNeedsInfo;
    public GameObject lakwatsaSlider;
    public GameObject lakwatsaInfo;
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

    public float delayBetweenReveals = 3.5f;

    // sliders info texts and explanations
    public TextMeshProUGUI OnhandcashText;
    public string OnhandCashExplanation = "This is the total money you have received from your allowance cut-off alongside any amount " +
        "that remained unspent from your previous daily needs allocation";
    public TextMeshProUGUI iponInfoText;
    public string iponExplanation = "Here you can adjust the amount you want to allocate to your Ipon goals towards that P80,000 mark. " +
        "The more you save, the higher your focus and Ipon goes!";
    public TextMeshProUGUI dailyNeedsInfoText;
    public string dailyNeedsExplanation = "This is where you adjust your budget for your daily needs (Obviously) like your meals, commutes and all that good stuff. " +
        "Don’t cheap out too much here, these are your necessities. Can’t have Alex fall into depression because you’re neglecting their needs!";
    public TextMeshProUGUI lakwatsaInfoText;
    public string lakwatsaExplanation = "Well this is everyone's favorite slider, you allocate your budget for your personal consumption and lakwatsa’s with friends. " +
        "But remember! You have a goal to achieve but also your happiness to look out for. Find the best medium to maintain Alex’s happiness and focus in school";

    // prompt text
    public TextMeshProUGUI promptText;
    public string promptMessage = "Your friends are planning a Pre-College staycation at Batangas to relax before school starts again and everyone gets too busy. " +
        "However, Alex is also considering selling cookies over the break to increase their savings. " +
        "Do you want to do one last lakwatsa, save your graduation gift, or start a mini-business over the summer as a head start towards your goal?";
    //public TextMeshProUGUI finalPromptText;
    public string finalPromptMessage = "Do you think Alex is responsible enough to budget if they choose to condo or dorm? Or would commuting home be a better choice to stray away from over spending?";

    // values info and explanations
    public TextMeshProUGUI socialText;
    public string socialExplanation = "Alex is a college student that needs to socialize every now and then. " +
        "Keep this at an optimal level and Alex will stay happy, " +
        "keep it too high Alex’s academic focus drops or keep it too low and Alex’s will fall into a downward spiral causing Alex to flunk and even drop out.";
    public TextMeshProUGUI happinessText;
    public string happinessExplanation = "This is Alex’s mental state. Try to keep it above half or as high as you can so that Alex can function properly and stay in focus with academics. " +
        "This increases when Alex takes a step closer to their goal, excels in school and gets to meet up with friends for a lakwatsa.";
    public TextMeshProUGUI focusText;
    public string focusExplanation = "Alex’s main priority is school. The focus bar is an indicator of Alex’s academic performance. " +
        "Its best to keep this as high as you can,  it can even open up some opportunities later on";
    public TextMeshProUGUI budgetGoalText;
    public string budgetGoalExplanation = "Pretty self explanatory really… But track your progress here and see how close (or far) you are from that 80K goal!";
    public TextMeshProUGUI currentCashText;
    public string currentCashExplanation = "This is your current cash on hand budgeted for your daily needs. If any is left unspent, it rolls over onto the next level together with your allowance.";

    void Update()
    {
        if (isTypingInfo && Input.GetKeyDown(KeyCode.Space))
        {
            skipTypewriter = true;
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
        //Debug.Log("Initial budget given: 5000");

        sliderGameObj.SetActive(true);

        // Show Cash On Hand
        cashOnHand.SetActive(true);
        cashOnHandInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(OnhandcashText, OnhandCashExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show Ipon slider & info
        iponSlider.SetActive(true);
        iponInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(iponInfoText, iponExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show Daily Needs slider & info
        dailyNeedsSlider.SetActive(true);
        dailyNeedsInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(dailyNeedsInfoText, dailyNeedsExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show Lakwatsa slider & info
        lakwatsaSlider.SetActive(true);
        lakwatsaInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(lakwatsaInfoText, lakwatsaExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        confirmButton.SetActive(true);

        // prompt to proceed
        sliderGameObj.SetActive(false);
        promptGameObj.SetActive(true);
        StartCoroutine(ShowPromptAndValues());
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

    IEnumerator ShowPromptAndValues()
    {
        // Typewriter for the prompt
        yield return StartCoroutine(TypeInfo(promptText, promptMessage));
        yield return new WaitForSeconds(4f); // or require spacebar to continue
        promptGameObj.SetActive(false);

        yield return StartCoroutine(RevealValues());
    }

    IEnumerator RevealValues()
    {
        values.SetActive(true);

        // Show social value & info
        social.SetActive(true);
        socialInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(socialText, socialExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show happiness value & info
        happiness.SetActive(true);
        happinessInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(happinessText, happinessExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show focus value & info
        focus.SetActive(true);
        focusInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(focusText, focusExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show budget goal value & info
        budgetGoal.SetActive(true);
        budgetGoalInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(budgetGoalText, budgetGoalExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        // Show current cash value & info
        currentCash.SetActive(true);
        currentCashInfo.SetActive(true);
        yield return StartCoroutine(TypeInfo(currentCashText, currentCashExplanation));
        yield return new WaitForSeconds(delayBetweenReveals);

        currentCashInfo.SetActive(false);
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
        // (Optionally) Play a staycation image, animation, or a line of narration
        // Animation/cutscene GameObject.SetActive(true)
        staycationAnim.SetActive(true);
        yield return new WaitForSeconds(2f); // Adjust for visual duration

        // Now proceed to FinalTutorialCutscene
        staycationAnim.SetActive(false);
        values.SetActive(false);
        promptGameObj.SetActive(false);
        dialogueManager.BeginFinalTutorialDialogue();
    }

    IEnumerator SavingAftermath()
    {
        // (Optionally) Play a staycation image, animation, or a line of narration
        // Animation/cutscene GameObject.SetActive(true)
        savingAnim.SetActive(true);
        Debug.Log("Playing saving aftermath animation");
        yield return new WaitForSeconds(2f); // Adjust for visual duration
        Debug.Log("done saving aftermath animation");

        // Now proceed to FinalTutorialCutscene
        savingAnim.SetActive(false);
        values.SetActive(false);
        promptGameObj.SetActive(false);
        dialogueManager.BeginFinalTutorialDialogue();
    }

    IEnumerator SellingAftermath()
    {
        // (Optionally) Play a staycation image, animation, or a line of narration
        // Animation/cutscene GameObject.SetActive(true)
        sellingAnim.SetActive(true);
        yield return new WaitForSeconds(2f); // Adjust for visual duration

        // Now proceed to FinalTutorialCutscene
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
        // Typewriter for the prompt
        yield return StartCoroutine(TypeInfo(promptText, finalPromptMessage));
        yield return new WaitForSeconds(4f); // or require spacebar to continue
        //promptGameObj.SetActive(false);

        choices.SetActive(true);
    }

    public void OnUwianChoice()
    {
        choices.SetActive(false);
        promptGameObj.SetActive(false);
        GameManager.Instance.chosenResidence = GameManager.ResidenceType.Uwian;
        //GameManager.Instance.SetBudget(15000);
        Debug.Log("Uwian chosen, budget set to 15000");

        StartCoroutine(UwianTransition());
    }
    public void OnCondoChoice()
    {
        choices.SetActive(false);
        promptGameObj.SetActive(false);
        GameManager.Instance.chosenResidence = GameManager.ResidenceType.Condo;
        //GameManager.Instance.SetBudget(24000);
        Debug.Log("Condo chosen, budget set to 24000");

        StartCoroutine(CondoTransition());
    }

    IEnumerator UwianTransition()
    {
        // (Optionally) Play a staycation image, animation, or a line of narration
        // Animation/cutscene GameObject.SetActive(true)
        uwianAnim.SetActive(true);
        yield return new WaitForSeconds(2f); // Adjust for visual duration

        // Now proceed to FinalTutorialCutscene
        uwianAnim.SetActive(false);
        SceneManager.LoadScene("Chapter 1");
    }
    IEnumerator CondoTransition()
    {
        // (Optionally) Play a staycation image, animation, or a line of narration
        // Animation/cutscene GameObject.SetActive(true)
        condoAnim.SetActive(true);
        yield return new WaitForSeconds(2f); // Adjust for visual duration

        // Now proceed to FinalTutorialCutscene
        condoAnim.SetActive(false);
        SceneManager.LoadScene("Chapter 1");
    }


}
