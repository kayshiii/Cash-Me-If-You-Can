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
    public float lineDelay = 2.5f; // seconds per line;
    public float typeSpeed = 0.035f;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private bool skipTypewriter = false;

    void Update()
    {
        // If typing, and space is pressed, complete instantly
        if (isTyping && Input.GetKeyDown(KeyCode.Space))
        {
            skipTypewriter = true;
        }
    }
    public void BeginDialogue()
    {
        currentIndex = 0;
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentIndex < dialogueLines.Length)
        {
            speakerText.text = dialogueLines[currentIndex].speaker;
            typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentIndex].text));
            yield return typingCoroutine;
            yield return new WaitForSeconds(lineDelay);
            currentIndex++;
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        skipTypewriter = false;
        dialogueText.text = "";

        foreach (char c in line)
        {
            if (skipTypewriter)
            {
                dialogueText.text = line; // show the rest immediately
                break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    
}
