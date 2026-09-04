using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event Action<Vector2> MoveInput;
    
    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        
        _playerInput.GameplayMain.Movement.performed += OnMovement;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        
        _playerInput.GameplayMain.Movement.performed += OnMovement;
    }
    
    private void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        MoveInput?.Invoke(inputVector);
    }
}
