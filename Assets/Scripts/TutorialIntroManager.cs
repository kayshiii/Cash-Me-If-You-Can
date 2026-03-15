using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialIntroManager : MonoBehaviour
{
    public CanvasGroup introGroup;
    public CanvasGroup fadeGroup;
    public GameObject dialoguePanel;
    public GameObject bgImg;
    public GameObject lolaBgImg;

    public float introDuration = 1.5f;
    public float fadeDuration = 1f;

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

        bgImg.SetActive(true);
        dialogueManager.BeginTutorialDialogue();
    }

    public IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        cg.gameObject.SetActive(true);
        cg.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        cg.alpha = to;
        if (Mathf.Approximately(to, 0f))
            cg.gameObject.SetActive(false);
    }
}
