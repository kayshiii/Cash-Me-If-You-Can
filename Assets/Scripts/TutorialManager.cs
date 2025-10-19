using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialIntroManager : MonoBehaviour
{
    public CanvasGroup introGroup;
    public GameObject dialoguePanel;
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
        Debug.Log("Starting Dialogue");

        dialogueManager.BeginTutorialDialogue();
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
