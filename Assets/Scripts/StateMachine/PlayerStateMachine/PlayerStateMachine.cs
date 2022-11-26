using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 playerVelocity;
    private bool playerGrounded;
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float gravityValue = -9.8f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravityMultiplier;
    private bool isJumping = false;
    private float initialJump = 0.3f;
    private float mayJump;
    private float gravity;
    public static PlayerStateMachine Instance { get; private set; }


    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        Jump();
        GroundingCheck();
        FallCheck();
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 move = new Vector3(input.x, playerVelocity.y, input.y);
        _characterController.Move(move * Time.deltaTime * playerSpeed);
    }

    public void JumpStart()
    {
        isJumping = true;
    }

    public void JumpFinished()
    {
        isJumping = false;
    }


    private void Jump()
    {
        if (isJumping)
        {
            if (playerGrounded || mayJump > 0 && playerVelocity.y < 0.5f)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }
    }

    private void GroundingCheck()
    {

        playerGrounded = _characterController.isGrounded;
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!playerGrounded)
        {
            mayJump -= Time.deltaTime;
        }
        else if (playerGrounded)
        {
            mayJump = initialJump;
        }
    }

    private void FallCheck()
    {
        gravity = gravityValue * gravityMultiplier;

        if (!isJumping && playerVelocity.y > 0)
        {
            gravity *= 2;
        }
        playerVelocity.y += gravity * Time.deltaTime;
    }

}
