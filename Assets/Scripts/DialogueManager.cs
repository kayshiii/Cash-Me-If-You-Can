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

    public GameObject phoneNotificationPanel;
    public AudioSource notificationSound;

    // Chapter 1 Dialogues
    public DialogueLine[] chapter1DialogueIntro;
    public DialogueLine[] chapter1DialogueBoyet;
    // Chapter 2 Dialogues
    public DialogueLine[] chapter2Dialogue;

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
        FindObjectOfType<Chapter1Manager>().LevelEndReport  ();
    }

    // --- CHAPTER 1 END ---

    // --- CHAPTER 2 ---
    public void BeginChapter2Intro()
    {
        StartCoroutine(chapter2DialogueIntro());
    }

    IEnumerator chapter2DialogueIntro()
    {
        dialoguePanel.SetActive(true);

        // Play first 3 dialogue lines (Alex, Boyet, Alex)
        for (int i = 0; i < 3; i++)
        {
            string speaker = chapter2Dialogue[i].speaker.ToLower().Trim();

            // Handle speaker icons
            if (speaker == "alex")
            {
                alexIcon.SetActive(true);
                boyetIcon.SetActive(false);
            }
            else if (speaker == "boyet")
            {
                alexIcon.SetActive(false);
                boyetIcon.SetActive(true);
            }

            speakerText.text = chapter2Dialogue[i].speaker;
            yield return StartCoroutine(TypeLine(chapter2Dialogue[i].text));
            yield return new WaitForSeconds(lineDelay);
        }

        // === PHONE NOTIFICATION PART ===
        // Play notification sound (optional)
        if (notificationSound != null)
        {
            notificationSound.Play();
        }

        // Show phone notification panel
        phoneNotificationPanel.SetActive(true);

        // Wait 5 seconds while notification is visible
        yield return new WaitForSeconds(5f);

        // Hide notification
        phoneNotificationPanel.SetActive(false);

        // === CONTINUE DIALOGUE ===
        // Play remaining dialogue (just "Wait lang, Boyet. BRB!")
        for (int i = 3; i < chapter2Dialogue.Length; i++)
        {
            string speaker = chapter2Dialogue[i].speaker.ToLower().Trim();

            if (speaker == "alex")
            {
                alexIcon.SetActive(true);
                boyetIcon.SetActive(false);
            }

            speakerText.text = chapter2Dialogue[i].speaker;
            yield return StartCoroutine(TypeLine(chapter2Dialogue[i].text));
            yield return new WaitForSeconds(lineDelay);
        }

        dialoguePanel.SetActive(false);
        alexIcon.SetActive(false);
        boyetIcon.SetActive(false);

        // When this finishes, show budget panel
        FindObjectOfType<Chapter2Manager>().ShowBudgetPanel();
    }
}
