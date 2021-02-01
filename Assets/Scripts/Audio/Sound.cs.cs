
//Code by Iván Aco august/10/2020

using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

//This class is the model of each sound in sound manager

[System.Serializable]
public class Sound 
{

    [Tooltip("Audio Name")]
	public string name;

    [Tooltip("Audioclips")]
	public List<AudioClip> clips = new List<AudioClip>();

    [Tooltip("Audio volume")]
	[Range(0f, 1f)]
	public float volume = .75f;

    [Tooltip("Audio pitch")]
	[Range(.1f, 3f)]
	public float pitch = 1f;

    [Tooltip("Will it be loop?")]
	public bool loop = false;

	//Audio group that modify the volume
	public AudioMixerGroup audioMixer;
    
	//Each sound has an audiosource
	[HideInInspector]
	public AudioSource audioSource;

    //AuxFlags
    //Flag used to control fade effect
	[HideInInspector]
    public bool fadingOut = false;
	[HideInInspector]
    public bool fadingIn = false;

	//Audio id
	[HideInInspector] public int id;

}