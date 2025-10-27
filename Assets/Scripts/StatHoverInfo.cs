using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StatHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum StatType { Happiness, Focus, Social }
    public StatType statType;
    public GameObject infoBox; // Assign in inspector (panel with text child)
    public TextMeshProUGUI infoText; // Assign the TMPUGUI inside infoBox

    public void OnPointerEnter(PointerEventData eventData)
    {
        string statName = "";
        int statValue = 0;
        int statMax = 100;

        switch (statType)
        {
            case StatType.Happiness:
                statName = "Happiness";
                statValue = GameManager.Instance.happiness;
                break;
            case StatType.Focus:
                statName = "Focus";
                statValue = GameManager.Instance.focus;
                break;
            case StatType.Social:
                statName = "Social";
                statValue = GameManager.Instance.social;
                break;
        }

        infoText.text = $"{statName}: {statValue} out of {statMax}";
        infoBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.SetActive(false);
    }
}
