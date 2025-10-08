using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(2, 5)]
    public string text;
}

public class DialogueManager : MonoBehaviour
{
    public DialogueLine[] dialogueLines;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public float lineDelay = 2.5f; // seconds per line; adjust as needed
    private int currentIndex = 0;

    /*void Start()
    {
        StartCoroutine(PlayDialogue());
    }*/

    public void BeginDialogue()
    {
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentIndex < dialogueLines.Length)
        {
            speakerText.text = dialogueLines[currentIndex].speaker;
            dialogueText.text = dialogueLines[currentIndex].text;
            yield return new WaitForSeconds(lineDelay);
            currentIndex++;
        }
        // Optionally: move to next scene or show a continue button
    }

    // Option: Add code here for input-based advancement
}
