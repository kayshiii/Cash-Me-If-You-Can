using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BudgetPanelManager : MonoBehaviour
{
    public Slider iponSlider;
    public Slider dailyNeedsSlider;
    public Slider lakwatsaSlider;

    public TextMeshProUGUI cashOnHandText;
    public TextMeshProUGUI iponGoalText;

    public GameObject budgetPanel;
    public GameObject spendingPanel;
    public GameObject negativeBudgetWarningPanel;

    private bool dailyNeedsUsed = false;

    private int startingBudget;

    void Start()
    {
        startingBudget = GameManager.Instance.budget;
        int dailyNeedsMin = GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo ? 15000 : 9000;
        dailyNeedsSlider.minValue = 0;
        iponSlider.maxValue = startingBudget;
        dailyNeedsSlider.maxValue = startingBudget;
        lakwatsaSlider.maxValue = startingBudget;

        UpdateAllDisplays();
    }
    public void InitializeSlidersToBudget()
    {
        startingBudget = GameManager.Instance.budget;

        int dailyNeedsMin = GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo ? 15000 : 9000;
        dailyNeedsSlider.minValue = 0;
        iponSlider.maxValue = startingBudget;
        dailyNeedsSlider.maxValue = startingBudget;
        lakwatsaSlider.maxValue = startingBudget;

        // Start at zero except daily needs set to min!
        iponSlider.value = 0;
        lakwatsaSlider.value = 0;
        dailyNeedsSlider.value = 0;
        dailyNeedsUsed = false;
        UpdateAllDisplays();
    }

    // Call this for each slider OnValueChanged, passing which slider was moved
    public void OnSliderChanged(string changedSlider)
    {
        // Clamp so total doesn't exceed budget
        int ipon = Mathf.RoundToInt(iponSlider.value);
        int needs = Mathf.RoundToInt(dailyNeedsSlider.value);
        int lakwatsa = Mathf.RoundToInt(lakwatsaSlider.value);

        int otherSum = 0;
        int newValue = 0;
        int available = startingBudget;

        if (changedSlider == "ipon")
        {
            otherSum = needs + lakwatsa;
            newValue = Mathf.Clamp(ipon, 0, startingBudget - otherSum);
            iponSlider.value = newValue;
        }
        else if (changedSlider == "needs")
        {
            int min = GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo ? 15000 : 9000;
            otherSum = ipon + lakwatsa;

            if (!dailyNeedsUsed && needs > 0)
            {
                // First movement: If above 0 but below min, snap to min
                needs = Mathf.Max(min, needs);
                dailyNeedsSlider.value = needs;
                dailyNeedsUsed = true;
            }
            else if (dailyNeedsUsed)
            {
                // Now always clamp between min and the max possible
                int maxPossible = startingBudget - otherSum;
                maxPossible = Mathf.Max(maxPossible, min);
                newValue = Mathf.Clamp(needs, min, maxPossible);
                dailyNeedsSlider.value = newValue;
            }
        }
        else if (changedSlider == "lakwatsa")
        {
            otherSum = ipon + needs;
            newValue = Mathf.Clamp(lakwatsa, 0, startingBudget - otherSum);
            lakwatsaSlider.value = newValue;
        }

        HideNegativeBudgetWarning();
        UpdateAllDisplays();
    }

    public void ResetSlidersForReallocation(bool isLakwatsaEvent, int lakwatsaMin)
    {
        // Always set ipon and needs to zero
        iponSlider.value = 0;
        dailyNeedsSlider.value = 0;

        // Lakwatsa: event-enforced minimum, else leave as zero
        lakwatsaSlider.minValue = isLakwatsaEvent ? lakwatsaMin : 0;
        lakwatsaSlider.value = isLakwatsaEvent ? lakwatsaMin : 0;

        dailyNeedsUsed = false;  // Reset state for daily needs slider for new session

        HideNegativeBudgetWarning();
        UpdateAllDisplays();
    }

    public void HideNegativeBudgetWarning()
    {
        if (negativeBudgetWarningPanel != null)
            negativeBudgetWarningPanel.SetActive(false);
    }

    public void UpdateAllDisplays()
    {
        int iponValue = Mathf.RoundToInt(iponSlider.value);
        int dailyNeedsValue = Mathf.RoundToInt(dailyNeedsSlider.value);
        int lakwatsaValue = Mathf.RoundToInt(lakwatsaSlider.value);

        int spentTotal = iponValue + dailyNeedsValue + lakwatsaValue;
        int cashRemaining = startingBudget - spentTotal;
        int totalIpon = GameManager.Instance.ipon + iponValue;

        cashOnHandText.text = cashRemaining.ToString("N0");
        iponGoalText.text = $"{totalIpon:N0} of 80,000";

        int minDailyNeeds = (int)dailyNeedsSlider.minValue;
        //warningText.gameObject.SetActive(dailyNeedsValue < minDailyNeeds);
    }

    public void ConfirmBudget()
    {
        int ipon = Mathf.RoundToInt(iponSlider.value);
        int dailyNeeds = Mathf.RoundToInt(dailyNeedsSlider.value);
        int lakwatsa = Mathf.RoundToInt(lakwatsaSlider.value);

        int spentTotal = ipon + dailyNeeds + lakwatsa;
        int cashRemaining = startingBudget - spentTotal;

        if (cashRemaining < 0)
        {
            // Show warning
            if (negativeBudgetWarningPanel != null)
                negativeBudgetWarningPanel.SetActive(true);
            Debug.LogWarning("Attempted to confirm budget with negative cash remaining.");
            return; // Do not proceed
        }

        // -- Save values to GameManager --
        GameManager.Instance.AddIpon(ipon);
        GameManager.Instance.dailyNeedsSpent = dailyNeeds;
        GameManager.Instance.SetBudget(startingBudget - (ipon + dailyNeeds + lakwatsa));
        Debug.Log("[CONFIRM] Saved: dailyNeedsSpent: " + GameManager.Instance.dailyNeedsSpent);

        // -- Transition panels --
        budgetPanel.SetActive(false);
        spendingPanel.SetActive(true);

        Debug.Log($"[CONFIRM] Saved: Ipon={ipon}, DailyNeeds={dailyNeeds}, Lakwatsa={lakwatsa}, Remaining={GameManager.Instance.budget}");
    }
}