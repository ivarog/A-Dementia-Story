
//Code by Ivan Aco 7/20/2020

using UnityEngine;
using System.Collections.Generic;


//Structure that defines the line of dialogue and who says it
[System.Serializable]
public class Line
{
    [Tooltip("Speaker")]
    public Character character;
    [Tooltip("Dialog line")]
    [TextArea(2, 5)]
    public string text;

    //Creation of a new line
    //@return line created
    public static Line CreateLine(Character character, string text)
    {
        Line newLine = new Line();
        newLine.character = character;
        newLine.text = text;
        return newLine;
    }
}

//Scriptable object that defines who are the characters that participate in the 
//conversation and the lines of dialogue of each one
[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue System/Conversation")]
public class Conversation : ScriptableObject
{
    [Header("Dialogue System")]

    [Tooltip("Characters to be displayed on the left side of the GUI")]
    public List<Character> speakersLeft;
    [Tooltip("Characters to be displayed on the right side of the GUI")]
    public List<Character> speakersRight;
    [Tooltip("Dialog lines")]
    public List<Line> lines;
    [Tooltip("Question after the conversation")]
    public Question question;
    [Tooltip("Conversation after this conversation")]
    public Conversation nextConversation;
    [Tooltip("Is checkpoint")]
    public bool isCheckpoint;

    //Auxiliar 
    bool oneCharacter = false;

    //Creation of a new conversation
    //@return conversation created
    public static Conversation CreateConversation(List<Character> speakersLeft, List<Character> speakersRight, List<Line> lines, Question question = null, Conversation nextConversation = null)
    {
        Conversation newConversation = new Conversation();
        newConversation.speakersLeft = speakersLeft;
        newConversation.speakersRight = speakersRight;
        newConversation.lines = lines;
        if(question != null) newConversation.question = question;
        if(nextConversation != null) newConversation.nextConversation = newConversation;
        return newConversation;
    }

    //Add line to conversation
    public void AddConversationLine(Line line)
    {
        this.lines.Add(line);
    }

    public void AddConversationLine(Line line, int position)
    {
        this.lines.Insert(position, line);
    }

    public void AddConversationLines(List<Line> lines)
    {
        this.lines.AddRange(lines);
    }

    //Add question to conversation
    public void AddQuestion(Question question)
    {
        this.question = question;
    }

    //This method serves as an auxiliary in the event that there is only one character 
    //to the left and zero to the right of the conversation, in this case it will be considered 
    //a monologue and eliminates any other character placed by mistake since it fills its Character 
    //in the ispector automatically
    private void OnValidate() {
        //If it´s monologue
        if(speakersRight.Count < 1)
        {
            //Fill with only the first character
            if(speakersLeft.Count == 1)
            {
                for(int i=0; i<lines.Count; i++)
                {
                    lines[i].character = speakersLeft[0];
                }
                oneCharacter = true;
            }
            //Empty array and fill with anyone
            else if(oneCharacter)
            {
                oneCharacter = false;
                for(int i=0; i<lines.Count; i++)
                {
                    lines[i].character = null;
                }
            }
        }
    }
}

