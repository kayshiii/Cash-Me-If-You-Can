using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoBox; // Assign this in the inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.SetActive(false);
    }
}
