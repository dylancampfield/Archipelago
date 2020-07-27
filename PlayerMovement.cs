using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    private Vector3 moveDirection;

    public float speed = 5f;
    private float gravity = 20f;

    public float jumpForce = 10f;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;

        ApplyGravity();

        controller.Move(moveDirection);


    }

    void ApplyGravity()
    {

        verticalVelocity -= gravity * Time.deltaTime;


        PlayerJump();

        moveDirection.y = verticalVelocity * Time.deltaTime;

    }

    void PlayerJump()
    {

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
        }

    }
}
