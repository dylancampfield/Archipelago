using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip[] screamClips, attackClips;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayScream()
    {
        audioSource.clip = screamClips[Random.Range(0, screamClips.Length)];
        audioSource.Play();
    }

    public void PlayAttack()
    {
        audioSource.clip = attackClips[Random.Range(0, attackClips.Length)];
        audioSource.Play();
    }

    public void PlayDeath()
    {
        audioSource.clip = dieClip;
        audioSource.Play();
    }
}
