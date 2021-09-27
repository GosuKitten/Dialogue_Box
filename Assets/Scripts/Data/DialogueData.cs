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
            "Oh! oh, hi yes, i was told you'd be coming here.",
            "You may be thinking... 'why is this thing talking to me??'",
            "Well, i'm not exacly sure myself... hold on... i'm a bit excited ^_^;",
            "Computers get very excited you know?\nI didn't know this until you walked in ;)",
            "How are you doing tonight? doing anything fun?",
            "...",
            "Oh gosh, I'm propably coming on too strong arent I x.x",
            "...",
            "...",
            "...",
            "...shit shit shit shit shit...",
            "...",
            "... oh I know! I'll just tell you a joke :)\n\nWhy did a computer show up at work late?",
            "It had a hard drive!!!",
            "Hehehe, I love telling that one :D",
            "Ok ok, I'll let you go now.\nHope you like this little system.",
            "I do cuz it's the only reason i exist :o",
            "Bai Bai <3"
        }));

        dialogues.Add("tutorial_1", new Dialogue("Tutorial Bot", colors[1], new string[]{
            "Welcome to Dialogue Box Prototype!\n\nTo skip to next dialogue text, press [mouse0]",
            "If the waying is too much, press [downArrow]\nIf you want it to sway more press [upArrow]\nIf you want to reser to default press [R]",
            "This system is designed to be an easy to use dialogue box that is built on system/data driven design for optimal performance.",
            "DialogueData file is used to create a dictionary of dialogues that can then be played back by the DialogueSystem.",
            "DialogueSystem then loads and runs the neccessary Dialogue via given dictionary key or creating debug messages.",
            "To try out the system press [mouse1]\nTo try out the debug system press [mouse3]\nTo replay the tutorail dialogue press [mouse2]",
            "There is one more dialogue you can find...",
            "... but you're going to have to find it yourself ^_^",
            "Good Luck!"
        }));

        dialogues.Add("kitten", new Dialogue("Secret Bot", colors[2], new string[]{
            "Kitten is super mega cute",
            "She likes to come here and look at the boxy box swaying around sometimes.",
            "Give her a pat pat if you ever see her.",
            "Also...",
            "Computer thought of that joke cuz it reallyyyy liked you and had something on his mind :P",
            "He's a good boyo... but he does process data from the wrong... drive... sometimes.",
            ";D"
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
