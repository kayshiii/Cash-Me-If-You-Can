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
        public int happinessChange;
        public int focusChange;
        public int socialChange;
        // public Sprite icon; // for later if you want visuals
    }

    private List<SpendingItem> cart = new List<SpendingItem>();
    private int cartTotal = 0;
    private int dailyNeedsAvailable = 0;

    [Header("Texts")]
    public TextMeshProUGUI dailyNeedsText;
    public TextMeshProUGUI cartCountText;

    public List<SpendingItem> condoItems;   // Setup in Inspector
    public List<SpendingItem> uwianItems;   // Setup in Inspector
    public GridLayoutGroup itemGrid;        // Assign the grid container
    public Button itemButtonPrefab;         // Assign a button prefab

    [Header("Panels")]
    public GameObject checkOutPanel;
    public GameObject spendingPanel;

    public Transform itemListParent; // content or vertical group in ScrollView
    public GameObject itemListEntryPrefab; // a prefab with just a TMP_Text or similar

    public TextMeshProUGUI checkoutTotalText;
    public TextMeshProUGUI checkoutRemainingText;

    private Dictionary<SpendingItem, bool> itemSelected = new Dictionary<SpendingItem, bool>();
    private Dictionary<Button, SpendingItem> buttonToItem = new Dictionary<Button, SpendingItem>();

    [Header("Scripts")]
    public GameObject cutscenePanel;
    public DialogueManager dialogueManager;
    public Chapter1Manager chapter1Manager;
    public Chapter2Manager chapter2Manager;
    public Chapter4Manager chapter4Manager;
    public Chapter5Manager chapter5Manager;
    public Chapter6Manager chapter6Manager;
    public Chapter7Manager chapter7Manager;
    public Chapter8Manager chapter8Manager;
    public Chapter9Manager chapter9Manager;
    public Chapter10Manager chapter10Manager;
    public Chapter11Manager chapter11Manager;
    public Chapter12Manager chapter12Manager;
    public Chapter13Manager chapter13Manager;
    public Chapter14Manager chapter14Manager;
    public Chapter15Manager chapter15Manager;
    public Chapter16Manager chapter16Manager;
    public Chapter17Manager chapter17Manager;
    public Chapter18Manager chapter18Manager;


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
                btn.image.color = Color.gray;
            }
            else
            {
                Debug.Log("Not enough daily needs budget!");
            }
        }
        else
        {
            cart.Remove(item);
            cartTotal -= item.cost;
            itemSelected[item] = false;
            btn.image.color = Color.white;
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

        // --- STAT CHANGES: Calculate and apply ---
        int netHappiness = 0;
        int netFocus = 0;
        int netSocial = 0;
        foreach (var item in cart)
        {
            netHappiness += item.happinessChange;
            netFocus += item.focusChange;
            netSocial += item.socialChange;
        }
        GameManager.Instance.AddHappiness(netHappiness);
        GameManager.Instance.AddFocus(netFocus);
        GameManager.Instance.AddSocial(netSocial);

        // Optionally: Debug log what happened
        Debug.Log($"Spending bonuses: +{netHappiness} happiness, +{netFocus} focus, +{netSocial} social");

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
        else if (GameManager.Instance.currentChapter == 4)
        {
            chapter4Manager.ProceedToCutscene();
        }
        else if (GameManager.Instance.currentChapter == 5)
        {
            chapter5Manager.ProceedAfterBudget5();
        }
        else if (GameManager.Instance.currentChapter == 6)
        {
            chapter6Manager.ProceedAfterBudget6();
        }
        else if (GameManager.Instance.currentChapter == 7)
        {
            chapter7Manager.ProceedAfterBudget7();
        }
        else if (GameManager.Instance.currentChapter == 8)
        {
            chapter8Manager.ProceedAfterBudget8();
        }
        else if (GameManager.Instance.currentChapter == 9)
        {
            chapter9Manager.ProceedAfterBudget9();
        }
        else if (GameManager.Instance.currentChapter == 10)
        {
            chapter10Manager.ProceedAfterBudget10();
        }
        else if (GameManager.Instance.currentChapter == 11)
        {
            chapter11Manager.ProceedAfterBudget11();
        }
        else if (GameManager.Instance.currentChapter == 12)
        {
            chapter12Manager.ProceedAfterBudget12();
        }
        else if (GameManager.Instance.currentChapter == 13)
        {
            chapter13Manager.ProceedAfterBudget13();
        }
        else if (GameManager.Instance.currentChapter == 14)
        {
            chapter14Manager.ProceedAfterBudget14();
        }
        else if (GameManager.Instance.currentChapter == 15)
        {
            chapter15Manager.ProceedAfterBudget15();
        }
        else if (GameManager.Instance.currentChapter == 16)
        {
            chapter16Manager.ProceedAfterBudget16();
        }
        else if (GameManager.Instance.currentChapter == 17)
        {
            chapter17Manager.ProceedAfterBudget17();
        }
        else if (GameManager.Instance.currentChapter == 18)
        {
            chapter18Manager.ProceedAfterBudget18();
        }
        // Add more chapters as needed

        Debug.Log($"Checkout confirmed. Remaining: {newDailyNeeds}. Proceeding to cutscene.");
    }
}
