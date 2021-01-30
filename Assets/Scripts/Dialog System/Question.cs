
//Code by Ivan Aco 7/23/2020

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
//Structure that defines every choice that question has
public class Choice
{
    [Tooltip("Question line")]
    [TextArea(2,5)]
    public string text;
    [Tooltip("Dialog line")]
    public Conversation conversation;

    //Creation of a new choice
    //@return choice created
    public static Choice CreateChoice(string text, Conversation conversation)
    {
        Choice newChoice = new Choice();
        newChoice.text = text;
        newChoice.conversation = conversation;
        return newChoice;
    }
}

//Scriptable object that defines a question in the conversation
[CreateAssetMenu(fileName = "New Question", menuName = "Dialogue System/Question")]
public class Question : ScriptableObject
{
    [Tooltip("Dialog line")]
    [TextArea(2, 5)]
    public string text;
    [Tooltip("Choices that player can have in the question")]
    public Choice[] choices;

    //Creation of a new question
    //@return question created
    public static Question CreateQuestion(string text, Choice[] choices)
    {
        Question question = new Question();
        question.text = text;
        question.choices = choices;
        return question;
    }
} 