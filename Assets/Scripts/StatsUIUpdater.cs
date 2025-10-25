using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUIUpdater : MonoBehaviour
{
    // Text references
    /*public TextMeshProUGUI socialText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI focusText;*/
    public TextMeshProUGUI budgetGoalText;
    public TextMeshProUGUI cashOnHandText;
    //public TextMeshProUGUI cashOnHand1Text;

    // Circle fill references
    public Image socialFillImage;
    public Image happinessFillImage;
    public Image focusFillImage;
    public Image iponFillImage;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update text values
        /*socialText.text = GameManager.Instance.social.ToString();
        happinessText.text = GameManager.Instance.happiness.ToString();
        focusText.text = GameManager.Instance.focus.ToString();*/
        budgetGoalText.text = $"{GameManager.Instance.ipon:N0} of 80,000";
        cashOnHandText.text = GameManager.Instance.budget.ToString("N0");
        //cashOnHand1Text.text = GameManager.Instance.budget.ToString("N0");

        // Update circle fills (assuming max value of 100 for each stat)
        socialFillImage.fillAmount = GameManager.Instance.social / 100f;
        happinessFillImage.fillAmount = GameManager.Instance.happiness / 100f;
        focusFillImage.fillAmount = GameManager.Instance.focus / 100f;
        iponFillImage.fillAmount = GameManager.Instance.ipon / 8000f;
    }
}
