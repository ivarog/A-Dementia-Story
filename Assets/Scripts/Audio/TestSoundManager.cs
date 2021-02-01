using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundManager : MonoBehaviour
{
    public void Play()
    {
        SoundManager.instance.Play("Hotline Miami");
    }

    public void Play2()
    {
        SoundManager.instance.Play("Hollow Knight");
    }

    public void StopAll()
    {
        SoundManager.instance.StopAllSounds();
    }

    public void ForcePlay()
    {
        SoundManager.instance.Play("Hotline Miami", true);
    }

    public void PlayGivenVolumePitch()
    {
        SoundManager.instance.Play("Hotline Miami", 0.8f, 1.3f, true);
    }

    public void PlayOneShot()
    {
        SoundManager.instance.PlayOneShot("Horn");
    }

    public void PlayOneShot2()
    {
        SoundManager.instance.PlayOneShot("1");
    }

    public void PlayFadeIn()
    {
        SoundManager.instance.PlayFadeIn("Hotline Miami", 5f, true);
    }

    public void Stop()
    {
        SoundManager.instance.Stop("Hotline Miami");
    }

    public void Pause()
    {
        SoundManager.instance.Pause("Hotline Miami");
    }

    public void StopFadeOut()
    {
        SoundManager.instance.StopFadeOut("Hotline Miami", 5f);
    }

    public void PauseFadeOut()
    {
        SoundManager.instance.PauseFadeOut("Hotline Miami", 5f);
    }
}
