using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [Header("Player Inputs")]
    private Player playerInput;
    private Player.MovementActions movement;
    private PlayerStateMachine playerStateMachine;





    public Player.MovementActions PlayerMovement{get{return movement;}}


    private void Awake()
    {
        Instance = this;
        playerInput = new Player();
        movement = new Player().Movement;
        playerStateMachine = GetComponent<PlayerStateMachine>();
        movement.PlayerJump.performed += ctx => playerStateMachine.Jump();
    }


    private void Update()
    {
        playerStateMachine.ProcessMove(movement.PlayerMovement.ReadValue<Vector2>());
        
    }


    private void OnEnable()
    {
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }
}
