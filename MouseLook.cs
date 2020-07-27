using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] private Transform player, look;
    [SerializeField] private bool invert;
    [SerializeField] private bool canUnlock = true; 
    [SerializeField] private float mouseSensitivity = 5f;
    //[SerializeField] private float smoothWeight = 0.4f;
    //[SerializeField] private float rollAngle = 1f;
    //[SerializeField] private float rollSpeed = 3f;
    //[SerializeField] private int smoothSteps = 10;

    private Vector2 lookAngles;
    private Vector2 currentMouseLook;
    //private Vector2 smoothMove;

    //private float currentRollAngle;
    //private int lastLookFrame;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LockAndUnlock(); 

        if(Cursor.lockState == CursorLockMode.Locked)
            LookAround();
    }

    //Unlock screen to show cursor
    void LockAndUnlock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LookAround()
    {
        //Getting "Mouse Y" rotation for horizontal(X) look angles,
        //and "Mouse X" rotation for vertical(Y) look angles
        currentMouseLook = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        lookAngles.x += currentMouseLook.x * mouseSensitivity * (invert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * mouseSensitivity;

        lookAngles.x = Mathf.Clamp(lookAngles.x, -60f, 75f);

        //currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw("Mouse X") * rollAngle, Time.deltaTime * rollSpeed);

        look.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);
        player.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }

}
