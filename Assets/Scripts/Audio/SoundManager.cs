
//Code by Iván Aco august/10/2020

using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

//This class has the methods that control the sounds

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;
    //Array of sounds used
	public SoundGroup[] soundGroups;
	//General Audiomixer
	public AudioMixer mixer;
	
    private Dictionary<int, Sound> soundsDictionary = new Dictionary<int, Sound>();

    //Used to diferentiate between atop and play in fade out effect
    private enum AudioAction{
        Pause,
        Stop
    }

	void Awake()
	{
        //Singleton pattern

		if (instance == null)
			instance = this;
        else
			Destroy(gameObject);

        //Initialize audiosources in the scene

		foreach (SoundGroup soundGroup in soundGroups)
		{
			foreach (Sound sound in soundGroup.soundsList)
			{
				sound.audioSource = gameObject.AddComponent<AudioSource>();
				sound.audioSource.clip = sound.clips[0];
				sound.audioSource.loop = sound.loop;
				sound.audioSource.playOnAwake = false;
				sound.audioSource.outputAudioMixerGroup = sound.audioMixer;
				sound.id = Animator.StringToHash(sound.name);
				soundsDictionary.Add(sound.id, sound);
			}
		}

	}
    
    //Play audio with given name with pitch and volume given in inspector
    //@params soundName name of audio to play
    //@params forcePlay if true, forces the audio to play again from the beginning
	public void Play(string soundName, bool forcePlay = false)
	{
        //Get sound with given name
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];

		if (sound == null)
		{
            #if UNITY_EDITOR
			    Debug.LogWarning("Sound: " + name + " not found!");
            #endif
			return;
		}

		if(forcePlay || !sound.audioSource.isPlaying)
		{
            //If fading out interrupt corountine and reset volume and pitch,
            // you dont need play it again else play
            if(sound.fadingOut)
                sound.fadingOut = false;
            else
			{
				int randomClip = UnityEngine.Random.Range(0, sound.clips.Count);
				sound.audioSource.clip = sound.clips[randomClip];
                sound.audioSource.Play();
			}

			//Adjust volume according to type of sound
			sound.audioSource.volume = sound.volume;

            sound.audioSource.pitch = sound.pitch;
		}
       
	}

    //Play audio with given name with pitch and volume given in inspector
    //@params soundName name of audio to play
    //@params volume volume audio will have overwrites given in inspector
    //@params pitch pitch audio will have overwrites given in inspector
    //@params forcePlay if true, forces the audio to play again from the beginning
	public void Play(string soundName, float volume, float pitch, bool forcePlay = false)
	{
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];

		if (sound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if(forcePlay || !sound.audioSource.isPlaying)
		{
            //If fading out interrupt corountine and reset volume and pitch, 
            //you dont need play it again else play
            if(sound.fadingOut)
                sound.fadingOut = false;
            else
			    sound.audioSource.Play();

			//Volume according type of sound

			sound.audioSource.volume = volume;

            sound.audioSource.pitch = pitch;
		}
	}

    //Play audio in one shot mode, several audios same time
    //@params soundName name of audio to play
    //@params forcePlay if true, forces the audio to play again from the beginning
	public void PlayOneShot(string soundName)
	{
        //Get sound with given name
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];

		if (sound == null)
		{
            #if UNITY_EDITOR
			    Debug.LogWarning("Sound: " + name + " not found!");
            #endif
			return;
		}

		sound.audioSource.volume = sound.volume;

		sound.audioSource.pitch = sound.pitch;
		if (sound.clips.Count > 1)
		{
			int randomClip = UnityEngine.Random.Range(0, sound.clips.Count);
			sound.audioSource.PlayOneShot(sound.clips[randomClip]);
		}
		else
		{
			sound.audioSource.PlayOneShot(sound.clips[0]);
		}
	}

    //Play audio with given fade in time
    //@params soundName name of audio to play
    //@params fadeTime timeFade in seconds
    //@params forcePlay if true, forces the audio to play again from the beginning
	public void PlayFadeIn(string soundName, float fadeTime, bool forcePlay = false)
	{
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];

		if (sound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}


		if(forcePlay || !sound.audioSource.isPlaying)
		{
            //If this is fading out, stop effect you dont need play because is playing
            // and reset volume and pitch
			sound.audioSource.volume = sound.volume;

		    sound.audioSource.pitch = sound.pitch;
            if(sound.fadingOut)
                sound.fadingOut = false;
            else
			    StartCoroutine(FadeIn(sound, fadeTime));
		}
	}

    //Pause audio with given name
    //@params soundName name of audio to pause
	public void Pause(string soundName)
	{
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];

		if (sound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if(sound.audioSource.isPlaying)
		{
			sound.audioSource.Pause();
		}
	}

    //Pause audio with given name
    //@params soundName name of audio to pause
	public void PauseFadeOut(string soundName, float fadeTime)
	{
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];

		if (sound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if(sound.audioSource.isPlaying)
		{
            //If this is fading in, stop effect and begin actual
            if(sound.fadingIn)
                sound.fadingIn = false;
			StartCoroutine(FadeOut(sound, fadeTime, AudioAction.Pause));
		}
	}

    //Stop audio with given name
    //@params soundName name of audio to stop
	public void Stop(string soundName)
	{
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];
		
		if (sound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if(sound.audioSource.isPlaying)
		{
			sound.audioSource.Stop();
		}
	}

    //Stop audio with given name
    //@params soundName name of audio to stop
    //@params fadeTime timeFade in seconds
	public void StopFadeOut(string soundName, float fadeTime)
	{
		Sound sound = soundsDictionary[Animator.StringToHash(soundName)];
		
		if (sound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if(sound.audioSource.isPlaying)
		{
            //If this is fading in, stop effect and begin actual
            if(sound.fadingIn)
                sound.fadingIn = false;
			StartCoroutine(FadeOut(sound, fadeTime, AudioAction.Stop));
		}
	}

	//Stop all audios in the scene
	public void StopAllSounds()
	{
		var allAudioSources = FindObjectsOfType<AudioSource>();
		foreach(AudioSource audioS in allAudioSources) 
		{
			audioS.Stop();
		}
	}

    //Corountine to create Fade Out effect used in stop and pause
    //@params Sound sound to handle
    //@params fadeTime timeFade in seconds
    //@params audioAction action to omplement between stop and pause
	private IEnumerator FadeOut(Sound sound, float FadeTime, AudioAction audioAction)
    {
        sound.fadingOut = true;

        while (sound.audioSource.volume > 0.0f)
        {
            if(!sound.fadingOut) break;

            sound.audioSource.volume -= sound.volume * Time.deltaTime / FadeTime;
            yield return null;
        }

        if(sound.fadingOut)
        {
            if(audioAction == AudioAction.Stop)
                sound.audioSource.Stop();
            else if(audioAction == AudioAction.Pause)
                sound.audioSource.Pause();

			sound.audioSource.volume = sound.volume;

        }

        sound.fadingOut = false;
		yield break;
    }
 
	//Corountine to create Fade In effect
    //@params Sound sound to handle
    //@params fadeTime timeFade in seconds
    private IEnumerator FadeIn(Sound sound, float FadeTime)
    {
        sound.fadingIn = true;

        sound.audioSource.volume = 0;
        sound.audioSource.Play();
 
        while (sound.audioSource.volume < sound.volume)
        {
            if(!sound.fadingIn) break;

			sound.audioSource.volume += sound.volume * Time.deltaTime / FadeTime;

            yield return null;
        }
 
        if(sound.fadingIn)
			sound.audioSource.volume = sound.volume;


        sound.fadingIn = false;

		yield break;
    }

	public void SetLevelVolume (string parameterName, float sliderValue)
    {
		//the logarithmic conversion will not work correctly at zero
		sliderValue = sliderValue < 0.0001f ? 0.0001f : sliderValue;
        mixer.SetFloat(parameterName, Mathf.Log10(sliderValue) * 20);
    }

}