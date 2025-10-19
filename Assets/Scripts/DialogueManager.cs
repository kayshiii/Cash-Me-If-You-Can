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
    public GameObject dialoguePanel;
    public DialogueLine[] tutorialDialogueLines;
    public DialogueLine[] finalTutorialDialogueLines;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public float lineDelay = 2.5f; // seconds per line;
    public float typeSpeed = 0.035f;

    //private int currentIndex = 0;
    private bool isTyping = false;
    //private Coroutine typingCoroutine;
    private bool skipTypewriter = false;

    public TutorialBudgetAlloc tutorialBudget;

    void Update()
    {
        if (isTyping && Input.GetKeyDown(KeyCode.Space))
        {
            skipTypewriter = true;
        }
    }

    public void BeginTutorialDialogue()
    {
        StartCoroutine(BeginTutorialFlow());
    }

    IEnumerator BeginTutorialFlow()
    {
        yield return StartCoroutine(PlayDialogueSequence(tutorialDialogueLines));
        dialoguePanel.SetActive(false);
        tutorialBudget.StartBudgetTutorial();
    }

    public void BeginFinalTutorialDialogue()
    {
        StartCoroutine(PlayFinalDialogueSequence(finalTutorialDialogueLines));
        // Add any logic for after cutscene here (next scene, menu, etc.)
    }

    public IEnumerator PlayDialogueSequence(DialogueLine[] dialogueSequence)
    {
        dialoguePanel.SetActive(true);
        for (int i = 0; i < dialogueSequence.Length; i++)
        {
            speakerText.text = dialogueSequence[i].speaker;
            yield return StartCoroutine(TypeLine(dialogueSequence[i].text));
            yield return new WaitForSeconds(lineDelay);
        }
        dialoguePanel.SetActive(false);
        //tutorialBudget.StartFinalPrompt();
    }
    public IEnumerator PlayFinalDialogueSequence(DialogueLine[] dialogueSequence)
    {
        dialoguePanel.SetActive(true);
        for (int i = 0; i < dialogueSequence.Length; i++)
        {
            speakerText.text = dialogueSequence[i].speaker;
            yield return StartCoroutine(TypeLine(dialogueSequence[i].text));
            yield return new WaitForSeconds(lineDelay);
        }
        dialoguePanel.SetActive(false);
        tutorialBudget.StartFinalPrompt();
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
                dialogueText.text = line;
                break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }


}
