
//Code by Ivan Aco 7/23/2020

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceController : MonoBehaviour
{

    [HideInInspector] public Choice choice;
    [Tooltip("Instance of conversation controller in the scene")]
    public ConversationController conversationController;
    [Tooltip("Instance of question controller in the scene")]
    public QuestionController questionController;

    //Add a button to the list of buttons in the question
    //@param choiceButtonTemplate template of the button to implement, the other buttons will be like this template
    //@param choice this object contains the choice information
    //@param index numer of choice in the list 
    public static ChoiceController AddChoiceButton(GameObject panel, Choice choice, int index, int panelMode)
    {
        Button button = null;

        if(panelMode == 1)
        {
            Button choiceButtonTemplate = panel.transform.Find("Choice Buttons/Choice Button").GetComponent<Button>();
            int buttonSpacing = -44;
            button = Instantiate(choiceButtonTemplate);

            //Button settings
            button.transform.SetParent(choiceButtonTemplate.transform.parent);
            button.transform.localScale = Vector3.one;
            button.transform.localPosition = new Vector3(0, index * buttonSpacing, 0);
            button.name = "Choice " + (index + 1);
            button.gameObject.SetActive(true);
        }
        else if(panelMode == 2 || panelMode == 3)
        {
            button = panel.transform.Find("Choice Buttons/Choice " + (index + 1)).GetComponent<Button>();
            button.gameObject.SetActive(true);
        }

        //Get ChoiceController
        ChoiceController choiceController = button.GetComponent<ChoiceController>();
        choiceController.choice = choice;
        button.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
        return choiceController;

    }

    private void Start() 
    {
        TextMeshProUGUI buttonTMP = GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
        //Get button text
        if(questionController.rigthToLeft)
            buttonTMP.isRightToLeftText = true;
        buttonTMP.text = choice.text;
        //If the instances are not assigned they will be searched in the scene
        if(conversationController == null) conversationController = FindObjectOfType<ConversationController>();   
        if(questionController == null) questionController = FindObjectOfType<QuestionController>();   
    }

    //Make choice on click button, the conversation is changed depending on the choice and the dialogue is hidden
    public void MakeChoice()
    {
        conversationController.ChangeConversation(choice.conversation);
        questionController.Hide();
    }
}