using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;   // <- multiple SFX

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MusicKey = "MusicVolume";
    private const string SfxKey = "SfxVolume";

    private void Start()
    {
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 1f);
        float savedSfx = PlayerPrefs.GetFloat(SfxKey, 1f);

        if (musicSlider != null) musicSlider.value = savedMusic;
        if (sfxSlider != null) sfxSlider.value = savedSfx;

        if (musicSource != null) musicSource.volume = savedMusic;
        SetSfxVolume(savedSfx);  // apply to all SFX [web:66]

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;

        PlayerPrefs.SetFloat(MusicKey, value);
    }

    public void SetSfxVolume(float value)
    {
        // Apply to all SFX AudioSources
        if (sfxSources != null)
        {
            for (int i = 0; i < sfxSources.Length; i++)
            {
                if (sfxSources[i] != null)
                    sfxSources[i].volume = value;
            }
        }

        PlayerPrefs.SetFloat(SfxKey, value);
    }
}
