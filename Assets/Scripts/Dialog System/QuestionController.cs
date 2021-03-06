
//Code by Ivan Aco 7/23/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionController : MonoBehaviour
{
    [Tooltip("Question to be displayed, can be null and setted by code")]
    public Question question;
    [Tooltip("Button template of the question")]
    public Button choiceButton;
    //there are three panels
    //Panel 1: It is a panel that is displayed in the middle of the screen and can accept more than 4 responses, 
    //the buttons are generated by code so it is longer to customize your position on the canvas 
    //Panel 2: It is the classic one of the convcersations of this type where the question panel is displayed 
    //directly in the dialog box
    //Panel 3: It is similar to the first but the buttons are generated from the GUI and the customization is easier
    [Tooltip("Panel to use")]
    [Range(1,3)]public int panelToUse;
    [Tooltip("Panel 1")]
    public GameObject panel1;
    [Tooltip("Panel 2")]
    public GameObject panel2;
    [Tooltip("Panel 3")]
    public GameObject panel3;

    //Text gameobject of question
    private TextMeshProUGUI questionText;
    //List that contains all choice controlles in cuttons of the question
    private List<ChoiceController> choiceControllers = new List<ChoiceController>();
    //Question has been started?
    private bool questionStarted = false;
    private GameObject actualPanel;
    public bool QuestionStarted { get{return questionStarted;} }

    public bool rigthToLeft = false;

    //Change question and pass to new question, Clean the actual question
    public void Change(Question newQuestion)
    {
        if(panelToUse == 1)
        {
            RemoveChoices();
        }

        question = newQuestion;
        gameObject.SetActive(true);

        if(panelToUse == 1)
        {
            actualPanel = panel1;
            questionText = panel1.transform.Find("Question").GetComponent<TextMeshProUGUI>();
        }
        else if(panelToUse == 2)
        {
            actualPanel = panel2;
            questionText = panel2.transform.Find("Question").GetComponent<TextMeshProUGUI>();
        }
        else if(panelToUse == 3)
        {
            actualPanel = panel3;
            questionText = panel3.transform.Find("Question").GetComponent<TextMeshProUGUI>();
        }
        
        actualPanel.SetActive(true);

        Initialize();
    }

    //Clean list of choices and hide gameobject
    public void Hide()
    {
        if(panelToUse == 1)
        {
            RemoveChoices();
        }
        
        questionStarted = false;
        actualPanel.SetActive(false);
    }

    //Delete choices in list of choices
    public void RemoveChoices()
    {
        foreach(ChoiceController c in choiceControllers)
        {
            Destroy(c.gameObject);
        }
        choiceControllers.Clear();
    }

    //Set the initial values of question
    private void Initialize()
    {
        if(rigthToLeft)
        {
            questionText.alignment = TextAlignmentOptions.TopRight;
            questionText.isRightToLeftText = true;
        }

            questionText.text = question.text;

        questionStarted = true;

        for(int index = 0; index < question.choices.Length; index++)
        {
            ChoiceController c = ChoiceController.AddChoiceButton(actualPanel, question.choices[index], index, panelToUse);
            choiceControllers.Add(c);
        }

        // choiceButton.gameObject.SetActive(false);
    }
}