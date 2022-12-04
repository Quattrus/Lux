using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 playerVelocity;
    [SerializeField] bool playerGrounded;
    private float playerInputX;
    private float playerInputY;
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float gravityValue = -9.8f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravityMultiplier;
    private bool isJumping = false;
    private float initialJump = 0.05f;
    private float mayJump;
    private float gravity;
    private bool isHealing;

    [SerializeField] float playerHealth = 100f;
    public static PlayerStateMachine Instance { get; private set; }
    public bool IsHealing { get { return isHealing; } set { isHealing = value; } }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        Jump();
        GroundingCheck();
        FallCheck();
        PlayerHealth();

        if ((_characterController.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (playerVelocity.y > 0)
            {
                playerVelocity.y = -playerVelocity.y;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        playerInputX = input.x;
        playerInputY = input.y;
        Vector3 move = new Vector3(playerInputX, playerVelocity.y, playerInputY);
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
            gravityValue = -9.8f;
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

    private void PlayerHealth()
    {
        if (isHealing && playerHealth < 100)
        {
            playerHealth = playerHealth + 10 * Time.deltaTime;
        }
        else if (!isHealing)
        {
            playerHealth = playerHealth - 1 * Time.deltaTime;
        }
        else
        {
            playerHealth = 100;
        }
        
    }
}
