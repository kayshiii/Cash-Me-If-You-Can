using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadingScript : MonoBehaviour
{
    public Image fadeImage; // Assign your black UI Image here in the Inspector
    public float fadeDuration = 1f; // 1 second fade
    public float holdDuration = 3f; // 5 seconds on screen
    //public string nextSceneName = "GameProperScene"; // Set your next scene here

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Fade in
        yield return StartCoroutine(Fade(1, 0, fadeDuration));
        // Hold
        yield return new WaitForSeconds(holdDuration);
        // Fade out
        yield return StartCoroutine(Fade(0, 1, fadeDuration));
        // Load next scene
        SceneManager.LoadScene("Tutorial");
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        Color color = fadeImage.color;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
