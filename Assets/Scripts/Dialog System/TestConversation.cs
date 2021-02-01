

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

//This class is used to test dialog system

[System.Serializable]
public class DementiaValues
{
    public float strength;
    public float time;
}

[System.Serializable]
public class Backgrounds
{
    public string conversationId;
    public Sprite backgroundSprite;
}

public class TestConversation : MonoBehaviour
{
    
    public ConversationController conversationController;
    public Conversation mainConversation;
    public Conversation lastConversationCheckpoint;
    public Conversation finishConversation;
    public Conversation finalConversation;

    public Material backgroundShader;
    public GameObject blackOutSquare;
    public GameObject titleScreen;
    public GameObject exitButton;


    public List<DementiaValues> dementiaValues;
    public List<Backgrounds> backgroundsValues;

    private int dementia;
    private int lastDementia;
    private bool canContinue = false;
    private bool finalOnce = false;

    public static TestConversation instance;
    
    void Awake(){

        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
    }

    void Start()
    {
        if(mainConversation)
        {
            conversationController.SetConversation(mainConversation);
            conversationController.AdvanceLine();
        }
        SoundManager.instance.Play("Main");
        SetDementiaValue(0);
        CheckBackground();
        StartCoroutine(TitleScreenState());
    }

    //Just used to advance the conversation state at mouse click
    private void Update() 
    {
        if(Input.GetMouseButtonDown(0) && canContinue)
        {
            conversationController.AdvanceLine();
            if(conversationController.actualConversation.isCheckpoint)
            {
                lastConversationCheckpoint = conversationController.actualConversation;
            }
            if(conversationController.actualConversation == finishConversation)
            {
                lastDementia = dementia;
                SetDementiaValue(3);
                canContinue = false;
                StartCoroutine("RestartMemories");
            }
        } 
        
        if(conversationController.actualConversation.isFinal && !finalOnce)
        {
            finalOnce = true;
            SoundManager.instance.StopFadeOut("Main", 4f);
            SoundManager.instance.PlayFadeIn("Final Music", 4f);
            exitButton.SetActive(true);
            backgroundShader.SetFloat("Vector1_C97773F6", 0f);
            backgroundShader.SetFloat("Vector1_C571230C", 0f);
        }
    }

    IEnumerator RestartMemories()
    {
        SoundManager.instance.StopFadeOut("Main", 4f);
        SoundManager.instance.PlayFadeIn("Noise", 4f);
        yield return new WaitForSeconds(2f);
        SetDementiaValue(4);
        yield return new WaitForSeconds(3f);
        
        blackOutSquare.SetActive(true);

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        float fadeSpeed = 1;

        while(blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + fadeSpeed * Time.deltaTime;
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }

        conversationController.ChangeConversation(lastConversationCheckpoint);
        conversationController.AdvanceLine();
        SetDementiaValue(1);

        while(blackOutSquare.GetComponent<Image>().color.a > 0){
            fadeAmount = objectColor.a - fadeSpeed * Time.deltaTime;
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }

        blackOutSquare.SetActive(false);

        canContinue = true;
        SoundManager.instance.Play("Main");
        SoundManager.instance.Stop("Noise");
        yield return null;
    }

    IEnumerator TitleScreenState()
    {
        yield return new WaitForSeconds(6f);
        titleScreen.SetActive(false);
        canContinue = true;
    }

    public void CheckBackground()
    {
        for(int i = 0; i < backgroundsValues.Count; i++)
        {
            if(backgroundsValues[i].conversationId == conversationController.actualConversation.name)
            {
                backgroundShader.SetTexture("Texture2D_B3DCF665", backgroundsValues[i].backgroundSprite.texture);
            }
        }
    }

    public void IncreaseDementia()
    {
        dementia = Mathf.Clamp(dementia + 1, 0, 3); 
        CheckDementiaValues();
        SoundManager.instance.PlayOneShot("Glitch");
    }

    public void DecreaseDementia()
    {
        dementia = Mathf.Clamp(dementia - 1, 0, 3); 
        CheckDementiaValues();
        SoundManager.instance.PlayOneShot("Click");
    }

    public void SetDementiaValue(int value)
    {
        dementia = value; 
        CheckDementiaValues();
    }

    public void CheckDementiaValues()
    {
        backgroundShader.SetFloat("Vector1_C97773F6", dementiaValues[dementia].strength);
        backgroundShader.SetFloat("Vector1_C571230C", dementiaValues[dementia].time);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
