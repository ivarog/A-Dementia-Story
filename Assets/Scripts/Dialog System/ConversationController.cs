
//Code by Ivan Aco 7/20/2020

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ConversationController  : MonoBehaviour{

    [Tooltip("Scriptable object that contains conversation")]
    public Conversation conversation;
    public Conversation actualConversation;
    [Tooltip("Object that controlls question")]
    public QuestionController questionController;
    

    [Tooltip("Gameobject containing the left speaker interface")]
    public GameObject speakerLeft;
    [Tooltip("Gameobject containing the right speaker interface")]
    public GameObject speakerRight;
    [Tooltip("Delay of each letter when displayed in dialogues")]
    public float textDelay = 0.05f;
    [Tooltip("Arabic notation?")]
    public bool rightToLeft = false;
    //If you have strange behaviors in the code, maybe you should decrease this value
    [Tooltip("Max number of character per line in box")]
    public int characterPerLine = 20;
    [Tooltip("Max number of lines in box")]
    public int numberOfLines = 4;

    //Speaker UI instances
    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;

    //Current line of conversation
    private int activeLineIndex = 0;

    //Is GUI displaying some text with typewritter effect?
    private bool displayingText = false;

    //Is conversation active?
    private bool conversationStarted = false;
    public bool ConversationStarted { get{return conversationStarted;} }


    //Get speakersUI instances and find quaetion controller if this is not given
    private void Awake() {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();    
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();    
        if(questionController == null) questionController = FindObjectOfType<QuestionController>();
    }
    
    //Set converation parameters
    //@param newConversation conversation scriptable object to be displayed
    public void SetConversation(Conversation newConversation){
        
        //Used to not modify original scriptableObject 
        //and modify a clon when CheckOverflowText in text makes an split
        List<Line> linesStored = newConversation.lines;
        List<Line> linesClone = new List<Line>();

        linesStored.ForEach((item) =>
        {
            Line newLine = Line.CreateLine(item.character, (string)item.text.Clone());
            linesClone.Add(newLine);
        });

        conversation = Conversation.CreateConversation(
            newConversation.speakersLeft, 
            newConversation.speakersRight, 
            linesClone,
            newConversation.question,
            newConversation.nextConversation
        );

        actualConversation = newConversation;
        
        //Set UI speakers
        speakerUILeft.Speakers = conversation.speakersLeft;
        speakerUIRight.Speakers = conversation.speakersRight;
        
        conversationStarted = true;
        activeLineIndex = 0;
    }

    //Delete conversation and return default values
    private void EndConversation()
    {
        conversation = null;
        conversationStarted = false;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
    }

    //Go to next conversation
    //@param nextConversation next conversation showed that will be showed
    public void ChangeConversation(Conversation nextConversation)
    {
        SetConversation(nextConversation);
        AdvanceLine();
    }

    //If the conversation has not ended, display the following line but the conversation ends.
    private void AdvanceConversation()
    {
        //If there is a question
        if(conversation.question != null)
        {
            //And it´s not started, start it
            if(!questionController.QuestionStarted)
            {
                questionController.Change(conversation.question);
                speakerUILeft.Hide();
                speakerUIRight.Hide();
            }
        }
        //If there is a new conversation
        else if(conversation.nextConversation != null)
        {
            ChangeConversation(actualConversation.nextConversation);
        }
        //Delete this conversation
        else
            EndConversation();

    }

    //Advance the lines in conversation, it also handles cases where the conversation has ended
    public void AdvanceLine()
    {
        //If there is not conversation setted then exit
        if(conversation == null) return;
        
        //It is used if on advanceConversation typewritter effect has not ended
        //With this check we finish the execution of typewritter effect and show the full text
        //See ShowText function for more details
        if(displayingText)
        {
            displayingText = false;
        }
        //Handle nex text in conversation 
        else 
        {
            if(activeLineIndex < conversation.lines.Count)
            {
                //Show next text and aument de dialog counter by one
                DisplayLine();
            }
            else
            {
                AdvanceConversation();
            }
        }
    }

    //Get the next line and select who will be the next to say it
    void DisplayLine(){
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;
        //If speaker in side is next then show GUI
        if(speakerUILeft.SpeakersHas(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text, character);
        } 
        else 
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text, character);
        }
        activeLineIndex += 1;
    }

    //Show/Hide speaker and set its attributes
    //@param activeSpeakerUI speaker that will be displayed in GUI
    //@param inactiveSpeakerUI speaker that will be hidden in GUI
    //@param text dialogue text
    //@param character actual character that says the line
    void SetDialog(SpeakerUI activeSpeakerUI, SpeakerUI inactiveSpeakerUI, string text, Character character){
        
        if(rightToLeft)
        {
            activeSpeakerUI.fullName.alignment = TextAlignmentOptions.MidlineRight;
            activeSpeakerUI.fullName.isRightToLeftText = true;   
            activeSpeakerUI.dialog.alignment = TextAlignmentOptions.TopRight;
            activeSpeakerUI.dialog.isRightToLeftText = true;   
        }

        StartCoroutine(ShowText(activeSpeakerUI, text, textDelay, character));

        activeSpeakerUI.FullName = character.fullName;
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();
    }

    //This function split the text in the dialog box to be sure that the overflow will be handled correctly
    //@param fulltext dialogue text
    //@param character actual character that says the line
    private string CheckOverflolwInText(string fullText, Character character)
    {
        string[] words = fullText.Split(' ');

        //Counter of max number of characters and lines
        int actualCharactersInLine = 0;
        int actualLines = 1;
        string overflow = "";
        bool panelFull = false;

        string textToDisplay = ""; 

        foreach(string word in words)
        {
            if(!panelFull)
            {
                //Check if a line break is needed
                if((actualCharactersInLine + word.Length) < characterPerLine)
                {
                    actualCharactersInLine += word.Length + 1;
                    textToDisplay += word + " ";
                }
                //You can still writte in this panel
                else if(actualLines < numberOfLines)
                {
                    actualLines++;
                    textToDisplay += "\n" + word + " ";
                    actualCharactersInLine = 0;
                    actualCharactersInLine += word.Length + 1;    
                }
                //You need other panel
                else
                {
                    panelFull = true;
                    overflow += word + " ";
                }

            }
            //Writte all next text in this string to create a new dialog
            else
                overflow += word + " ";
        }

        //Create a new dialog line to show overflow
        if(overflow.Length > 1)
        {
            conversation.lines[activeLineIndex].text = textToDisplay;
            Line overflowLine = Line.CreateLine(character, overflow);
            conversation.AddConversationLine(overflowLine, activeLineIndex + 1);
        }

        //Text that will be displayed
        return textToDisplay;
    }

    //Function used to create typewritter effect
    //@param activeSpeakerUI speaker that will be displayed in GUI
    //@param fulltext dialogue text
    //@param delay delay of each letter when displayed in dialogues
    //@param character actual character that says the line
	IEnumerator ShowText(SpeakerUI activeSpeakerUI, string fullText, float delay, Character character)
    {   

        displayingText = true;

        string textToDisplay = CheckOverflolwInText(fullText, character);
        
		for(int i = 0; i <= textToDisplay.Length; i++)
        {
            if(displayingText)
            {
			    activeSpeakerUI.Dialog = textToDisplay.Substring(0,i);
			    yield return new WaitForSeconds(delay);
            }
            else
            {
                activeSpeakerUI.Dialog = textToDisplay;
                break;
            }
		}

        displayingText = false;
	}


}

