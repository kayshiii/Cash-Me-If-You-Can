using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialIntroManager : MonoBehaviour
{
    public CanvasGroup introGroup; // Add CanvasGroup to IntroPanel and assign
    public GameObject dialoguePanel; // Dialogue box UI panel
    public float introDuration = 2f;
    public float fadeDuration = 2f;

    public DialogueManager dialogueManager;

    void Start()
    {
        dialoguePanel.SetActive(false);
        StartCoroutine(ShowIntroThenDialogue());
    }

    IEnumerator ShowIntroThenDialogue()
    {
        yield return StartCoroutine(Fade(introGroup, 0, 1, fadeDuration)); // Fade in
        yield return new WaitForSeconds(introDuration); // Stay
        yield return StartCoroutine(Fade(introGroup, 1, 0, fadeDuration)); // Fade out
        introGroup.gameObject.SetActive(false);
        dialoguePanel.SetActive(true); // Show dialogue
        // Trigger dialogue system here (call your dialogue function)
        dialogueManager.BeginDialogue();
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = to;
    }
}
