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

    private bool isTyping = false;
    private bool skipTypewriter = false;

    // character icons
    public GameObject alexIcon;
    public GameObject parentsIcon;
    public GameObject momIcon;
    public GameObject dadIcon;
    public GameObject boyetIcon;

    // Chapter 1 Dialogues
    public DialogueLine[] chapter1DialogueIntro; // Alex before Budget Sliders
    public DialogueLine[] chapter1DialogueBoyet; // Alex meets Boyet

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
            string speaker = dialogueSequence[i].speaker.ToLower().Trim();
            if (speaker == "alex")
            {
                alexIcon.SetActive(true);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
            }
            else if (speaker == "alex's parents")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(true);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
            }
            else if (speaker == "mom")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(true);
                dadIcon.SetActive(false);
            }
            else if (speaker == "dad")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(true);
                momIcon.SetActive(false);
            }
            else if (speaker == "boyet")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(true);
            }

            else
            {
                // No icon or add more as needed
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(false);
                momIcon.SetActive(false);
            }

            speakerText.text = dialogueSequence[i].speaker;
            yield return StartCoroutine(TypeLine(dialogueSequence[i].text));
            yield return new WaitForSeconds(lineDelay);
        }
        dialoguePanel.SetActive(false);
        alexIcon.SetActive(false);
        parentsIcon.SetActive(false);
    }
    public IEnumerator PlayFinalDialogueSequence(DialogueLine[] dialogueSequence)
    {
        dialoguePanel.SetActive(true);
        for (int i = 0; i < dialogueSequence.Length; i++)
        {
            string speaker = dialogueSequence[i].speaker.ToLower().Trim();
            if (speaker == "alex")
            {
                alexIcon.SetActive(true);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
            }
            else if (speaker == "alex's parents")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(true);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
            }
            else if (speaker == "mom")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(true);
                dadIcon.SetActive(false);
            }
            else if (speaker == "dad")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(true);
                momIcon.SetActive(false);
            }
            else if (speaker == "boyet")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(true);
            }

            else
            {
                // No icon or add more as needed
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(false);
                momIcon.SetActive(false);
            }

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

    // --- CHAPTER 1 ---
    // Begin first cutscene (Alex getting ready)
    public void BeginChapter1Intro()
    {
        StartCoroutine(PlayChapter1Intro());
    }

    // Begin Boyet meeting cutscene
    public void BeginBoyetDialogue()
    {
        StartCoroutine(PlayBoyetDialogue());
    }

    IEnumerator PlayChapter1Intro()
    {
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(PlayDialogueSequence(chapter1DialogueIntro));

        // When this finishes, trigger budget phase
        FindObjectOfType<Chapter1Manager>().ShowBudgetPanel();
    }

    IEnumerator PlayBoyetDialogue()
    {
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(PlayDialogueSequence(chapter1DialogueBoyet));


        // When this finishes, show level report
        FindObjectOfType<Chapter1Manager>().LevelEndReport();
    }

}
