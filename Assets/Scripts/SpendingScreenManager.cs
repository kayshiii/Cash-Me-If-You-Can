using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpendingScreenManager : MonoBehaviour
{
    [System.Serializable]
    public class SpendingItem
    {
        public string itemName;
        public int cost;
        // public Sprite icon; // for later if you want visuals
    }

    private List<SpendingItem> cart = new List<SpendingItem>();
    private int cartTotal = 0;
    private int dailyNeedsAvailable = 0;

    public TextMeshProUGUI dailyNeedsText;
    public TextMeshProUGUI cartCountText;

    public List<SpendingItem> condoItems;   // Setup in Inspector
    public List<SpendingItem> uwianItems;   // Setup in Inspector
    public GridLayoutGroup itemGrid;        // Assign the grid container
    public Button itemButtonPrefab;         // Assign a button prefab

    public GameObject checkOutPanel;
    public GameObject spendingPanel;

    public Transform itemListParent; // content or vertical group in ScrollView
    public GameObject itemListEntryPrefab; // a prefab with just a TMP_Text or similar

    public TextMeshProUGUI checkoutTotalText;
    public TextMeshProUGUI checkoutRemainingText;

    private Dictionary<SpendingItem, bool> itemSelected = new Dictionary<SpendingItem, bool>();
    private Dictionary<Button, SpendingItem> buttonToItem = new Dictionary<Button, SpendingItem>();

    public GameObject cutscenePanel; // assign in inspector
    public DialogueManager dialogueManager;
    public Chapter1Manager chapter1Manager;
    public Chapter2Manager chapter2Manager;


    void Start()
    {
        dailyNeedsAvailable = GameManager.Instance.dailyNeedsSpent;
        Debug.Log("SpendingScreenManager sees dailyNeedsSpent: " + GameManager.Instance.dailyNeedsSpent);
        List<SpendingItem> items = (GameManager.Instance.chosenResidence == GameManager.ResidenceType.Condo)
            ? condoItems : uwianItems;

        foreach (var item in items)
        {
            Button btn = Instantiate(itemButtonPrefab, itemGrid.transform);
            SpendingItem capturedItem = item;
            Button capturedBtn = btn;

            itemSelected[capturedItem] = false;
            buttonToItem[capturedBtn] = capturedItem;

            btn.onClick.AddListener(() => {
                ToggleCartItem(capturedItem, capturedBtn);
            });

            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.itemName} ({item.cost})";
        }

        UpdateNeedsUI();
        UpdateCartCountUI();
    }

    public void AddToCart(SpendingItem item)
    {
        Debug.Log($"{item.itemName} button pressed!");

        if (cartTotal + item.cost <= dailyNeedsAvailable)
        {
            cart.Add(item);
            cartTotal += item.cost;
            UpdateNeedsUI();
            UpdateCartCountUI();
        }
        else
        {
            Debug.Log("Not enough daily needs budget!");
            // Optionally, give player feedback!
        }
    }

    void UpdateNeedsUI()
    {
        // Show remaining daily needs value
        dailyNeedsText.text = (dailyNeedsAvailable - cartTotal).ToString("N0");
        // Optionally update a cart icon, etc.
    }

    void UpdateCartCountUI()
    {
        cartCountText.text = $"{cart.Count}";
    }
    public void ToggleCartItem(SpendingItem item, Button btn)
    {
        // Toggle logic
        if (!itemSelected[item]) // select (add)
        {
            // Only add if enough funds available
            if (cartTotal + item.cost <= dailyNeedsAvailable)
            {
                cart.Add(item);
                cartTotal += item.cost;
                itemSelected[item] = true;
                btn.image.color = Color.gray; // or any color for visual feedback
            }
            else
            {
                Debug.Log("Not enough daily needs budget!");
                // Optionally display a warning to the player.
            }
        }
        else // deselect (remove)
        {
            cart.Remove(item);
            cartTotal -= item.cost;
            itemSelected[item] = false;
            btn.image.color = Color.white; // revert to original color
        }
        UpdateNeedsUI();
        UpdateCartCountUI();
    }

    public void GoToCheckOut()
    {
        // Hide the main spending grid, show checkout
        this.gameObject.SetActive(false);
        checkOutPanel.SetActive(true);

        RenderCheckoutList();
    }
    public void BackToSpendingScreen()
    {
        checkOutPanel.SetActive(false);
        spendingPanel.SetActive(true);
        // Optionally re-initialize the spending UI if needed
    }

    void RenderCheckoutList()
    {
        // Clear previous list first:
        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);

        int currentTotal = 0;
        foreach (var item in cart)
        {
            GameObject entry = Instantiate(itemListEntryPrefab, itemListParent);
            entry.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.itemName}    -{item.cost:N0}";
            currentTotal += item.cost;
        }

        // Show current total spent
        checkoutTotalText.text = $"Total: {currentTotal:N0}";
        // Show remaining
        checkoutRemainingText.text = $"Daily Needs Left: {dailyNeedsAvailable - currentTotal:N0}";
    }

    public void OnConfirmCheckout()
    {
        // Calculate what remains
        int newDailyNeeds = dailyNeedsAvailable - cartTotal;
        GameManager.Instance.dailyNeedsSpent = newDailyNeeds;

        // Key: Add any daily needs leftover back to the main budget
        if (newDailyNeeds > 0)
            GameManager.Instance.AddBudget(newDailyNeeds);

        // Update total rollover for next level:
        GameManager.Instance.previousLevelRemainingCash = GameManager.Instance.budget;

        checkOutPanel.SetActive(false);

        // Route based on current chapter
        if (GameManager.Instance.currentChapter == 1)
        {
            chapter1Manager.ProceedToCutscene();
        }
        else if (GameManager.Instance.currentChapter == 2)
        {
            chapter2Manager.ProceedToCutscene();
        }
        // Add more chapters as needed

        Debug.Log($"Checkout confirmed. Remaining: {newDailyNeeds}. Proceeding to cutscene.");
    }
}
