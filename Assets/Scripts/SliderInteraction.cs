using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderInteraction : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private bool showDecimal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Reset()
    {
        slider = GetComponent<Slider>();
        valueText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void HandleSliderVlue()
    {
        if(showDecimal)
        {
            valueText.text = slider.value.ToString("0.00");
        }
        else
        {
            valueText.text = slider.value.ToString("0");
        }
    }
}
