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

    public static PlayerStateMachine Instance { get; private set; }


    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        playerGrounded = _characterController.isGrounded;
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        Debug.Log(playerVelocity.y);
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 move = new Vector3(input.x, playerVelocity.y, input.y);
        _characterController.Move(move * Time.deltaTime * playerSpeed);
    }
    public void Jump()
    {
        if (playerGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue * 0.5f);
        }
      
    }

}
