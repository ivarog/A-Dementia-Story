
//Code by Ivan Aco 7/20/2020

using UnityEngine;

//A scriptableobject is defined to define a character in the dialog system with its name and sprite used.
[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue System/Character")]
public class Character : ScriptableObject
{
    [Header("Character characteristics")]

    [Tooltip("Character Name")]
    public string fullName;
    [Tooltip("Character Sprite")]
    public Sprite portrait;

    //Creation of a new Character
    //@return character created
    public static Character CreateCharacter(string fullName, Sprite portrait)
    {
        Character newCharacter = new Character();
        newCharacter.fullName = fullName;
        newCharacter.portrait = portrait;
        return newCharacter;
    }
}
