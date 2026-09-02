using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private CharacterAnimator _animator;
    
    private PlayerInput _playerInput;

    private Vector2 _moveInputDirection;
    private Vector2 _lookInputDirection;

    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput; 

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        
        _playerInput.GameplayMain.Movement.performed += OnMovement;
        _playerInput.GameplayMain.Look.performed += OnLook;
        _playerInput.GameplayMain.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        
        _playerInput.GameplayMain.Movement.performed -= OnMovement;
        _playerInput.GameplayMain.Look.performed -= OnLook;
        _playerInput.GameplayMain.Attack.performed -= OnAttack;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInputDirection = context.ReadValue<Vector2>();
        
        LookInput?.Invoke(_lookInputDirection);
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _moveInputDirection = context.ReadValue<Vector2>();
        
        MoveInput?.Invoke(_moveInputDirection);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attacking");
    }
}
