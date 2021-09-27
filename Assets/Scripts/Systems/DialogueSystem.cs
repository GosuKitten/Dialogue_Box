using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueData;

public class DialogueSystem : MonoBehaviour
{
    // static instance for singularity and ease of reference
    public static DialogueSystem instance;

    // reference to the dialogueData class for dialogue lookups
    Dictionary<string, Dialogue> dialogueData;

    // dialogue queue
    Queue<string> dialogueQueue = new Queue<string>();

    // should the dialogue be playing?
    bool playingDialogue = false;

    // GO containing all dialogue box canvas items
    GameObject container;
    // all text components
    Text dialogueSourceText;
    Text dialogueText;
    Text continueText;
    // next dialogue text
    string nextText = "";

    // character delay veriables
    public float characterDelay = .05f;
    float characterTimer;

    // character skip veriables
    public float skipDelay = .1f;
    float skipTimer = 0;

    // color veriables
    Image[] colorElements;
    public Color debugColor;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // get references to all needed objects
        dialogueData = FindObjectOfType<DialogueData>().dialogues;
        dialogueSourceText = GameObject.Find("DialogueSource").GetComponent<Text>();
        dialogueText = GameObject.Find("Dialogue").GetComponent<Text>();
        continueText = GameObject.Find("Continue").GetComponent<Text>();

        // set character timer to delay so the first letter goes immediately
        characterTimer = characterDelay;

        // get remaining objects
        container = GameObject.Find("DialogueContainer").gameObject;
        Image contineBG = GameObject.Find("ContinueBox").GetComponent<Image>();

        // place all Image collor changing items into an array
        colorElements = new Image[] {container.GetComponent<Image>(), contineBG };

        HideDialogueBox();
    }

    /// <summary>
    /// Loads dialogue as per dialogue name in DialogueData class into dialogue queue and displays it on screen
    /// </summary>
    /// <param name="dialogueName">Dialogue name in DialogueData class</param>
    public void LoadDialogue(string dialogueName)
    {
        Dialogue data;
        // tryget the dialogue info from Dialogue Data class
        if (dialogueData.TryGetValue(dialogueName, out data))
        {
            // loop through each color element
            foreach (Image i in colorElements)
            {
                i.color = data.borderColor;
            }

            // set source name text color and text
            dialogueSourceText.color = data.borderColor;
            dialogueSourceText.text = $"> {data.sourceName}";
            // clear dialogue text
            dialogueText.text = "";

            // clear dialogue queue and enqueue new text from dialogue data
            dialogueQueue.Clear();
            for (int i = 0; i < data.dialogueText.Length; i++)
            {
                dialogueQueue.Enqueue(data.dialogueText[i]);
            }

            // start dialogue in case it's not shown
            ShowDialogueBox();
        }
        else
        {
            // show debug message about incorrect dialogue name
            ShowDebug($"Tried to get dialogue with name '{dialogueName}' which does not exist");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // testing stuff
        if (Input.GetMouseButtonDown(1))
        {
            LoadDialogue("computer_hello");
        }

        if (Input.GetMouseButtonDown(3))
        {
            ShowDebug($"Kitten is cute");
        }

        if (Input.GetMouseButtonDown(4))
        {
            ShowDebug(new string[] { "Kitten is actually gay", "She likes to come here and look at the boxy box swaying around.", "There is no helping her :P"});
        }

        // progress dialogue
        if (playingDialogue)
        {
            // if player can skip
            if (skipTimer >= skipDelay)
            {
                // and player pressed the skip button
                if (Input.GetMouseButtonDown(0))
                {
                    // see what the next text should be and reset skip timer
                    NextText();
                    skipTimer = 0;
                }
            }
            else
            {
                skipTimer += Time.deltaTime;
            }

            // increment characters onto dialogue text one by one
            IncrementText();
        }
    }

    void IncrementText()
    {
        // if the nextText has more characters
        if (nextText.Length != 0)
        {
            // and it's time to show the next character
            if (characterTimer >= characterDelay)
            {
                // add the first character of the nextText to dialogue text and remove it frim nextText
                dialogueText.text += nextText[0];
                nextText = nextText.Remove(0, 1);
                characterTimer = 0;
                // max out skip timer

                // show aproperiate continue icon
                if(nextText.Length == 0)
                {
                    skipTimer = skipDelay;
                    if (dialogueQueue.Count == 0)
                    {
                        // close icon
                        continueText.text = "-x-";
                    }
                    else
                    {
                        // next icon
                        continueText.text = "-->";
                    }
                }
            }
            else
            {
                characterTimer += Time.deltaTime;
            }
        }
    }

    public void NextText()
    {
        // if nextText has more characters to go
        if (nextText.Length != 0)
        {
            // add all the text into the dialogue and clear nextText
            dialogueText.text += nextText;
            nextText = "";

            // show aproperiate continue icon
            if(dialogueQueue.Count != 0)
            {
                // next icon
                continueText.text = "-->";
            }
            else
            {
                // close icon
                continueText.text = "-x-";
            }
        }
        else
        {
            // if there is more dialogue in the queue
            if (dialogueQueue.Count != 0)
            {
                // reset dialogue text and deque next text
                dialogueText.text = "";
                nextText = dialogueQueue.Dequeue();
                // skip icon
                continueText.text = ">>";
            }
            else
            {
                HideDialogueBox();
            }
        }
    }

    void ShowDialogueBox()
    {
        // set container active to show dialogue box
        container.SetActive(true);

        // start playing dialogue 
        playingDialogue = true;

        // set initial values
        skipTimer = 0;
        nextText = "";
        NextText();
    }

    void HideDialogueBox()
    {
        // set container inactive to hide dialogue box
        container.SetActive(false);
        // stop dialogue from playing
        playingDialogue = false;
        
    }

    /// <summary>
    /// Show debug dialogue with a single message
    /// </summary>
    /// <param name="message">Debug message</param>
    void ShowDebug(string message)
    {
        // loop through each color element
        foreach (Image i in colorElements)
        {
            i.color = debugColor;
        }

        // set source name text color and text
        dialogueSourceText.color = debugColor;
        dialogueSourceText.text = "> System Debug";
        // clear dialogue text
        dialogueText.text = "";

        // clear dialogue queue and enqueue new text from provided message
        dialogueQueue.Clear();
        dialogueQueue.Enqueue(message);

        // start dialogue in case it's not shown
        ShowDialogueBox();
    }

    /// <summary>
    /// Show debug dialogue with a multiple messages
    /// </summary>
    /// <param name="messages">Debug messages</param>
    void ShowDebug(string[] messages)
    {
        // loop through each color element
        foreach (Image i in colorElements)
        {
            i.color = debugColor;
        }

        // set source name text color and text
        dialogueSourceText.color = debugColor;
        dialogueSourceText.text = "> System Debug";
        // clear dialogue text
        dialogueText.text = "";

        // clear dialogue queue and enqueue new text from provided messages
        dialogueQueue.Clear();
        for (int i = 0; i < messages.Length; i++)
        {
            dialogueQueue.Enqueue(messages[i]);
        }

        // start dialogue in case it's not shown
        ShowDialogueBox();
    }
}
