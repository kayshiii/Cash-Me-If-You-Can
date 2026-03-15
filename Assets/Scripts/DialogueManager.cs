using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    public DialogueLine[] tutorialLolaLines;
    public DialogueLine[] finalTutorialDialogueLines;
    public TextMeshProUGUI speakerText;
    //public TextMeshProUGUI dialogueText;

    public float lineDelay = 0.5f; // seconds per line;
    public float typeSpeed = 0.001f;

    private bool isTyping = false;
    private bool skipTypewriter = false;
    private bool lineFinished = false;
    private bool proceedToNextLine = false;

    // character icons
    public GameObject alexIcon;
    public GameObject parentsIcon;
    public GameObject momIcon;
    public GameObject dadIcon;
    public GameObject boyetIcon;
    public GameObject lolaIcon;

    // text bubbles
    public GameObject textBubAlex;
    public TextMeshProUGUI textBubAlexText;
    public GameObject textBub;
    public TextMeshProUGUI textBubText;

    //pop up
    public GameObject popupPanel;
    public GameObject moneyIcon;
    public TextMeshProUGUI popUpText;
    public float moneyPopupDuration = 2f;

    //next icon
    public GameObject nextIcon;

    public GameObject phoneNotificationPanel;
    public AudioSource notificationSound;

    // Chapter 1 Dialogues
    public DialogueLine[] chapter1DialogueIntro;
    public DialogueLine[] chapter1DialogueBoyet;
    // Chapter 2 Dialogues
    public DialogueLine[] chapter2Dialogue;
    // Chapter 2 Dialogues
    public DialogueLine[] chapter3Intro;
    public DialogueLine[] chapter3Lola;

    public TutorialBudgetAlloc tutorialBudget;
    public Chapter3Manager chapter3Manager;
    public TutorialIntroManager introManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // First press while typing → reveal instantly
                skipTypewriter = true;
            }
            else
            {
                proceedToNextLine = true;
            }
        }
    }

    public void BeginTutorialDialogue()
    {
        StartCoroutine(BeginTutorialFlow());
    }

    IEnumerator BeginTutorialFlow()
    {
        yield return StartCoroutine(PlayDialogueSequence(tutorialDialogueLines));
        yield return StartCoroutine(introManager.Fade(introManager.fadeGroup, 0f, 1f, introManager.fadeDuration));
        introManager.bgImg.SetActive(false);
        introManager.lolaBgImg.SetActive(true);
        yield return StartCoroutine(introManager.Fade(introManager.fadeGroup, 1f, 0f, introManager.fadeDuration));
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(PlayDialogueSequence(tutorialLolaLines));
        dialoguePanel.SetActive(false);
        tutorialBudget.StartBudgetTutorial();
    }

    public void BeginFinalTutorialDialogue()
    {
        StartCoroutine(PlayFinalDialogueSequence(finalTutorialDialogueLines));
    }

    public IEnumerator PlayDialogueSequence(DialogueLine[] dialogueSequence)
    {
        dialoguePanel.SetActive(true);
        textBubAlex.SetActive(false);
        textBub.SetActive(false);

        for (int i = 0; i < dialogueSequence.Length; i++)
        {
            string speaker = dialogueSequence[i].speaker.ToLower().Trim();
            if (speaker == "popup")
            {
                popupPanel.SetActive(true);
                moneyIcon.SetActive(true);
                popUpText.gameObject.SetActive(true);

                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(false);
                lolaIcon.SetActive(false);

                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);
                textBubText.gameObject.SetActive(false);
                textBub.SetActive(false);

                popUpText.text = dialogueSequence[i].text;
                //popUpText.text = "";

                yield return new WaitForSeconds(moneyPopupDuration);
                popupPanel.SetActive(false);
                moneyIcon.SetActive(false);
                popUpText.gameObject.SetActive(false);

                continue; // Skip normal dialogue handling for this line
            }
            else if (speaker == "alex")
            {
                alexIcon.SetActive(true);
                textBub.SetActive(true);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(false);
                lolaIcon.SetActive(false);

                textBubAlexText.gameObject.SetActive(true);
                textBubAlex.SetActive(true);
                textBubText.gameObject.SetActive(false);
                textBub.SetActive(false); 

                textBubAlexText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubAlexText, dialogueSequence[i].text));
            }
            else if (speaker == "alex's parents")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(true);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "mom")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(true);
                dadIcon.SetActive(false);
                lolaIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "dad")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(true);
                momIcon.SetActive(false);
                lolaIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "boyet")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(true);
                lolaIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "lola mom")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(false);
                lolaIcon.SetActive(true);

                textBubAlexText.gameObject.SetActive(true);
                textBubAlex.SetActive(true);
                textBubText.gameObject.SetActive(false);
                textBub.SetActive(false);

                textBubAlexText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubAlexText, dialogueSequence[i].text));
            }

            else
            {
                // No icon or add more as needed
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(false);
                momIcon.SetActive(false);
                lolaIcon.SetActive(false);
            }

            //yield return new WaitForSeconds(lineDelay);
        }
        dialoguePanel.SetActive(false);
        alexIcon.SetActive(false);
        parentsIcon.SetActive(false);
    }
    public IEnumerator PlayFinalDialogueSequence(DialogueLine[] dialogueSequence)
    {
        introManager.lolaBgImg.SetActive(false);
        introManager.bgImg.SetActive(true);
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
                lolaIcon.SetActive(false);

                textBubAlexText.gameObject.SetActive(true);
                textBubAlex.SetActive(true);
                textBubText.gameObject.SetActive(false);
                textBub.SetActive(false);

                textBubAlexText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubAlexText, dialogueSequence[i].text));
            }
            else if (speaker == "alex's parents")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(true);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "mom")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(true);
                dadIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "dad")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(true);
                momIcon.SetActive(false);
                lolaIcon.SetActive(false);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }
            else if (speaker == "boyet")
            {
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                momIcon.SetActive(false);
                dadIcon.SetActive(false);
                boyetIcon.SetActive(true);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, dialogueSequence[i].text));
            }

            else
            {
                // No icon or add more as needed
                alexIcon.SetActive(false);
                parentsIcon.SetActive(false);
                dadIcon.SetActive(false);
                momIcon.SetActive(false);
            }

            yield return new WaitForSeconds(lineDelay);
        }
        dialoguePanel.SetActive(false);
        tutorialBudget.StartFinalPrompt();
    }

    IEnumerator TypeLine(TextMeshProUGUI target, string line)
    {
        isTyping = true;
        skipTypewriter = false;
        lineFinished = false;
        proceedToNextLine = false;
        target.text = "";

        // hide icon while typing
        if (nextIcon != null) nextIcon.SetActive(false);

        foreach (char c in line)
        {
            if (skipTypewriter)
            {
                target.text = line;
                break;
            }
            target.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;

        // show icon when ready to go next
        if (nextIcon != null) nextIcon.SetActive(true);

        // Wait for Space/click
        yield return new WaitUntil(() => proceedToNextLine);
        proceedToNextLine = false;

        // hide again right after consuming the input
        if (nextIcon != null) nextIcon.SetActive(false);
    }

    // --- CHAPTER 1 ---
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

                textBubAlexText.gameObject.SetActive(true);
                textBubAlex.SetActive(true);
                textBubText.gameObject.SetActive(false);
                textBub.SetActive(false);

                textBubAlexText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubAlexText, chapter2Dialogue[i].text));
            }
            else if (speaker == "boyet")
            {
                alexIcon.SetActive(false);
                boyetIcon.SetActive(true);

                textBubText.gameObject.SetActive(true);
                textBub.SetActive(true);
                textBubAlexText.gameObject.SetActive(false);
                textBubAlex.SetActive(false);

                textBubText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubText, chapter2Dialogue[i].text));
            }

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

                textBubAlexText.gameObject.SetActive(true);
                textBubAlex.SetActive(true);
                textBubText.gameObject.SetActive(false);
                textBub.SetActive(false);

                textBubAlexText.text = ""; // Reset bubble text

                nextIcon.SetActive(false);
                yield return StartCoroutine(TypeLine(textBubAlexText, chapter2Dialogue[i].text));
            }

            yield return new WaitForSeconds(lineDelay);
        }

        dialoguePanel.SetActive(false);
        alexIcon.SetActive(false);
        boyetIcon.SetActive(false);

        // When this finishes, show budget panel
        FindObjectOfType<Chapter2Manager>().ShowBudgetPanel();
    }

    // --- CHAPTER 2 END ---

    // --- CHAPTER 3 ---
    public void BeginChapter3Intro()
    {
        StartCoroutine(PlayChapter3Dialogue());
    }

    IEnumerator PlayChapter3Dialogue()
    {
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(PlayDialogueSequence(chapter3Intro));

        chapter3Manager.ShowBudgetPanel();
    }

    public void BeginChapter3Lola()
    {
        StartCoroutine(PlayChapter3Lola());
    }
    IEnumerator PlayChapter3Lola()
    {
        dialoguePanel.SetActive(true);
        yield return StartCoroutine(PlayDialogueSequence(chapter3Lola));

        chapter3Manager.StartPhoneSeq();
    }
    // --- CHAPTER 3 END ---
}
