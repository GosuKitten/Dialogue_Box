using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    public Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();
    // Start is called before the first frame update

    public Color[] colors;

    private void Awake()
    {
        AddComputerDialogue();
    }

    void AddComputerDialogue()
    {
        dialogues.Add("computer_hello", new Dialogue("Computer", colors[0], new string[]{
        "Hello, I am here to help you.",
        "You may be thinking... 'why is my computer be talking to me?'",
        "Computers get very excited you know? I didnt know this until you walked in ;)"
        }));
    }

    public struct Dialogue
    {
        public string sourceName;
        public string[] dialogueText;
        public Color borderColor;

        public Dialogue(string _sourceName, Color _borderColor, string[] _dialogueText)
        {
            sourceName = _sourceName;
            borderColor = _borderColor;
            dialogueText = _dialogueText;
        }
    }
}
