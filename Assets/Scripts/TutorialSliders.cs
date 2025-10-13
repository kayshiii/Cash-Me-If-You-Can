using UnityEngine;
using System.Collections;
using TMPro;

public class TutorialSliders : MonoBehaviour
{
    public GameObject sliderGroup; // Assign the parent group in inspector
    public GameObject cashOnHand;
    public GameObject cashOnHandInfo;
    public GameObject iponSlider;
    public GameObject iponInfo;
    public GameObject dailyNeedsSlider;
    public GameObject dailyNeedsInfo;
    public GameObject lakwatsaSlider;
    public GameObject lakwatsaInfo;

    public float delayBetweenReveals = 3.5f;

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

    public void StartBudgetTutorial()
    {
        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        sliderGroup.SetActive(true);

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
        // ...and so on (add more wait if needed)
    }

    IEnumerator TypeInfo(TextMeshProUGUI infoText, string infoString)
    {
        infoText.text = "";
        foreach (char c in infoString)
        {
            infoText.text += c;
            yield return new WaitForSeconds(0.035f); // typing speed
        }
    }
}
