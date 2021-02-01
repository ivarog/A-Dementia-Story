using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float minWaitTime = 3;
    public float maxWaitTime = 5;

    private AudioSource audioSource;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(UpdateSound());
    }

    IEnumerator UpdateSound()
    {
        yield return new WaitForSeconds(0.5f);
        
        if(!audioSource.isPlaying)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            PlaySound();
        }

        StartCoroutine(UpdateSound());
    }

    private void PlaySound()
    {
        audioSource.clip = audioClips[Random.Range(0,audioClips.Length)];
        audioSource.Play();
    }

}
