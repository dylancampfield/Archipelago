using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClip;
    [HideInInspector] public float volMin, volMax;
    [HideInInspector] public float stride; //Essentially the length of a stride when Walking or Sprinting or Crouching

    private AudioSource footstepSound;
    private CharacterController controller;
    
    private float distanceMoved; //How far player must move before footstep sound is played


    
    void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        controller = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayFootsteps();
    }

    //Is player moving? Should footsteps be playing?
    void PlayFootsteps()
    {
        if (!controller.isGrounded) //If not on the ground...
            return; //Exit

        if(controller.velocity.sqrMagnitude > 0) //If moving...
        {
            distanceMoved += Time.deltaTime; 
            //Checking the amount of distance moved against the determined
            //"lenght of stride" (stepDistance) to indicate when to play
            //footstep sounds to coordinate with foot hitting the ground
            if(distanceMoved > stride)
            {
                footstepSound.volume = Random.Range(volMin, volMax); //Set volume between min and max
                footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)]; //Set clip to random clip in the array
                footstepSound.Play();

                distanceMoved = 0f;
            }
        } else {
            distanceMoved = 0f;
        }
    }
}