

using UnityEngine;
using System.Collections.Generic;

//This class is used to test dialog system

public class TestConversation : MonoBehaviour
{
    
    public ConversationController conversationController;
    public Conversation mainConversation;
    public Conversation lastConversationCheckpoint;
    public Conversation finishConversation;

    void Start()
    {
        if(mainConversation)
        {
            conversationController.SetConversation(mainConversation);
        }
    }

    //Just used to advance the conversation state at mouse click
    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            conversationController.AdvanceLine();
            if(conversationController.actualConversation.isCheckpoint)
            {
                lastConversationCheckpoint = conversationController.actualConversation;
            }
            if(conversationController.actualConversation == finishConversation)
            {
                conversationController.SetConversation(lastConversationCheckpoint);
            }
        } 
    }

}
