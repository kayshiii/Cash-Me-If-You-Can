using UnityEngine;
using TMPro;

public class StatsUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI socialText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI focusText;
    public TextMeshProUGUI budgetGoalText; // e.g., "0 of 80,000"
    public TextMeshProUGUI cashOnHandText; // e.g., "10,000"
    public TextMeshProUGUI cashOnHand1Text; // e.g., "10,000"

    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        socialText.text = GameManager.Instance.social.ToString();
        happinessText.text = GameManager.Instance.happiness.ToString();
        focusText.text = GameManager.Instance.focus.ToString();
        budgetGoalText.text = $"{GameManager.Instance.ipon:N0} of 80,000";
        cashOnHandText.text = GameManager.Instance.budget.ToString("N0");
        cashOnHand1Text.text = GameManager.Instance.budget.ToString("N0");
    }
}
