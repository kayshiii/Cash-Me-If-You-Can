using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Player stats
    public int budget = 0;
    public int happiness = 0;
    public int social = 0;
    public int focus = 0;
    public int ipon = 0;
    public int lakwatsa = 0;

    // Tracking
    public int currentChapter = 1;
    public int previousLevelRemainingCash = 0;
    public int dailyNeedsSpent = 0; 

    // Residence choice
    public enum ResidenceType { None, Uwian, Condo }
    public ResidenceType chosenResidence = ResidenceType.None;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        happiness = 50;
        social = 50;
        focus = 50;
    }

    // --- Debug-Friendly Stat Methods ---

    public void AddBudget(int amount)
    {
        int prev = budget;
        budget += amount;
        Debug.Log($"[Budget] {prev} {(amount >= 0 ? "+" : "")}{amount} = {budget}");
    }
    public void SetBudget(int value)
    {
        int prev = budget;
        budget = value;
        Debug.Log($"[Budget] {prev} -> {budget} (Set)");
    }
    public void AddHappiness(int amount)
    {
        int prev = happiness;
        happiness += amount;
        Debug.Log($"[Happiness] {prev} {(amount >= 0 ? "+" : "")}{amount} = {happiness}");
    }
    public void SetHappiness(int value)
    {
        int prev = happiness;
        happiness = value;
        Debug.Log($"[Happiness] {prev} -> {happiness} (Set)");
    }
    public void AddSocial(int amount)
    {
        int prev = social;
        social += amount;
        Debug.Log($"[Social] {prev} {(amount >= 0 ? "+" : "")}{amount} = {social}");
    }
    public void SetSocial(int value)
    {
        int prev = social;
        social = value;
        Debug.Log($"[Social] {prev} -> {social} (Set)");
    }
    public void AddFocus(int amount)
    {
        int prev = focus;
        focus += amount;
        Debug.Log($"[Focus] {prev} {(amount >= 0 ? "+" : "")}{amount} = {focus}");
    }
    public void SetFocus(int value)
    {
        int prev = focus;
        focus = value;
        Debug.Log($"[Focus] {prev} -> {focus} (Set)");
    }

    public void AddIpon(int amount)
    {
        int prev = ipon;
        ipon += amount;
        Debug.Log($"[Ipon] {prev} {(amount >= 0 ? "+" : "")}{amount} = {ipon}");
    }
    public void SetIpon(int value)
    {
        int prev = ipon;
        ipon = value;
        Debug.Log($"[Ipon] {prev} -> {ipon} (Set)");
    }
    public void AddLakwatsa(int amount)
    {
        int prev = lakwatsa;
        ipon += amount;
        Debug.Log($"[lakwatsa] {prev} {(amount >= 0 ? "+" : "")}{amount} = {lakwatsa}");
    }
    public void SetLakwatsa(int value)
    {
        int prev = lakwatsa;
        lakwatsa = value;
        Debug.Log($"[Lakwatsa] {prev} -> {lakwatsa} (Set)");
    }

    // For reset/set all
    public void ResetStats()
    {
        budget = 0;
        happiness = 0;
        social = 0;
        focus = 0;
        ipon = 0;
        currentChapter = 1;
        Debug.Log("[GameManager] Stats reset to zero and chapter 1.");
    }

    public void SetStats(int newBudget, int newHappiness, int newSocial, int newFocus, int newIpon)
    {
        budget = newBudget;
        happiness = newHappiness;
        social = newSocial;
        focus = newFocus;
        ipon = newIpon;
        Debug.Log($"[SetStats] Budget: {budget}, Happiness: {happiness}, Social: {social}, Focus: {focus}, Ipon: {ipon}");
    }
}
