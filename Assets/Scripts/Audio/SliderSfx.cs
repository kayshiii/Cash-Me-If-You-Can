using UnityEngine;
using UnityEngine.UI;   // For Slider

public class SliderSfx : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sliderClip;
    [SerializeField] private Slider slider;

    private float lastValue;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        lastValue = slider.value;

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Optional: only play if value changed enough to avoid spam
        if (Mathf.Abs(value - lastValue) > 0.01f)
        {
            if (audioSource != null && sliderClip != null)
            {
                audioSource.PlayOneShot(sliderClip);
            }
            lastValue = value;
        }
    }
}
