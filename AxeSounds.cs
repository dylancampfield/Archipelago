using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] axeSounds;

    void PlaySounds()
    {
        audioSource.clip = axeSounds[Random.Range(0, axeSounds.Length)];
        audioSource.Play();
    }
}
