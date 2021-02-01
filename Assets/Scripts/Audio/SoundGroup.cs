//Code by Iván Aco august/10/2020

using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundGroup", menuName = "Audio Manager/SoundGroup")]
public class SoundGroup : ScriptableObject 
{
    public List<Sound> soundsList = new List<Sound>();
}
