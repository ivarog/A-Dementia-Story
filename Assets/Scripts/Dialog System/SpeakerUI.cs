
//Code by Ivan Aco 7/20/2020

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SpeakerUI : MonoBehaviour{


    [Tooltip("Speaker Name Container")]
    public TextMeshProUGUI fullName;
    [Tooltip("Speaker Dialog Container")]
    public TextMeshProUGUI dialog;

    //Setters and Getters of variables
    private List<Character> speakers;
    public List<Character> Speakers{
        get{ return speakers; }
        set {
            speakers = value;
        }
    }

    public string Dialog{
        set { dialog.text = value; }
        get { return dialog.text; }
    }

    public string FullName{
        set { fullName.text = value; }
        get { return fullName.text; }
    }


    //@return instance has speakers associated?
    public bool HasSpeaker(){
        return speakers != null;
    }

    //@param Character searched in speakers array
    //@return the instance has in its speakers the mentioned character?
    public bool SpeakersHas(Character character)
    {
        for (int i = 0; i < speakers.Count; i++)
        {
            if (speakers[i] == character) return true;
        }
        return false;
    }

    //Show this gameobject
    public void Show(){
        gameObject.SetActive(true);
    }

    //Hide this gamobject
    public void Hide(){
        gameObject.SetActive(false);
    }
}