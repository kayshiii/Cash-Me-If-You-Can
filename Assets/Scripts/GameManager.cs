using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Example player stats
    public int playerBudget;
    public int playerHappiness;
    public int playerSocial;
    public int playerFocus;

    public int currentChapter = 0; // optional

    void Awake()
    {
        // Ensure only one instance persists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Add methods for updating stats, resetting for new game, etc.
}
