using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public AudioClip[] clips;

    public AudioSource audioSource;

    public void Play()
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
