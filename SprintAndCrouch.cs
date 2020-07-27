using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAndCrouch : MonoBehaviour {

    private PlayerMovement playerMovement;
    private Transform playerEyes;
    private PlayerFootsteps playerFootsteps;
    private PlayerStats playerStats;

    private float standHeight = 1.6f;
    private float crouchHeight = .8f;
    private float sprintVolume = 1f;
    private float crouchVolume = .1f;
    private float walkVolumeMin = .2f;
    private float walkVolumeMax = .6f;
    private float walkStride = .4f;
    private float sprintStride = .25f;
    private float crouchStride = .6f;
    private float staminaValue = 100f;
    private bool isCrouching; 

    public float sprintSpeed = 8f;
    public float moveSpeed = 4f;
    public float crouchSpeed = 1.5f;
    public float staminaThreshold = 10f; //Subtract from stamina value
      

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerEyes = transform.GetChild(0); //Gets "Look" GameObject, the first child of the "Player" GameObject
        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();
        playerStats = GetComponent<PlayerStats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerFootsteps.stride = walkStride;
        playerFootsteps.volMin = walkVolumeMin;
        playerFootsteps.volMax = walkVolumeMax;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if(staminaValue > 0f) //Must have stamina to sprint
        {   //TODO: Prevent stamina from decreasing when player isn't moving
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching) //Holding LShift and not crouching, run
            { 
                playerMovement.speed = sprintSpeed;
                playerFootsteps.stride = sprintStride;
                playerFootsteps.volMin = sprintVolume;
                playerFootsteps.volMax = sprintVolume;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching) //Not holding LShift and not crouching, return to move speed 
        { 
            playerMovement.speed = moveSpeed;
            playerFootsteps.stride = walkStride;
            playerFootsteps.volMin = walkVolumeMin;
            playerFootsteps.volMax = walkVolumeMax;
            
        }

        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching) //Running
        {
            staminaValue -= staminaThreshold * Time.deltaTime; //Deplete stamina

            if(staminaValue <= 0f)
            {
                staminaValue = 0f; //Keep from going below 0

                //Reset speed and sounds
                playerMovement.speed = moveSpeed;
                playerFootsteps.stride = walkStride;
                playerFootsteps.volMin = walkVolumeMin;
                playerFootsteps.volMax = walkVolumeMax;
            }

            playerStats.DisplayStaminaStats(staminaValue);
        }
        else
        {
            if(staminaValue != 100f) //If stamina not at 100 and run key isn't held, recharge stamina
            {
                staminaValue += (staminaThreshold / 2f) * Time.deltaTime; //Recharge is 2x slower than expenditure

                playerStats.DisplayStaminaStats(staminaValue);

                if(staminaValue > 100f)
                {
                    staminaValue = 100f; //Reset to 100
                }
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C)) //Toggle crouch
        { 
            if (isCrouching) //If currently crouching, stand up
            { 
                playerEyes.localPosition = new Vector3(0f, standHeight, 0f);
                playerMovement.speed = moveSpeed;
                playerFootsteps.stride = walkStride;
                playerFootsteps.volMin = walkVolumeMin;
                playerFootsteps.volMax = walkVolumeMax;

                isCrouching = false;
            }
            else //Else, crouch
            { 
                playerEyes.localPosition = new Vector3(0f, crouchHeight, 0f);
                playerMovement.speed = crouchSpeed;
                playerFootsteps.stride = crouchStride;
                playerFootsteps.volMin = crouchVolume;
                playerFootsteps.volMax = crouchVolume;

                isCrouching = true;
            }
        }
    }
}
