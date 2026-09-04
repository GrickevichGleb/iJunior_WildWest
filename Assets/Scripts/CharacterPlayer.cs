using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharacterMover _mover;
    [SerializeField] private CharacterCamera _characterCamera;
    
    private void OnEnable()
    {
        _inputReader.MoveInput += OnMoveInput;
        _inputReader.LookInput += OnLookInput;

        _inputReader.Aiming += OnAiming;
    }

    private void OnDisable()
    {
        _inputReader.MoveInput -= OnMoveInput;
        _inputReader.LookInput -= OnLookInput;
        
        _inputReader.Aiming -= OnAiming;
    }

    private void OnMoveInput(Vector2 moveInput)
    {
        _mover.Move(moveInput);
    }

    private void OnAiming(bool isAiming)
    {
        _characterCamera.SwitchAimCamera(isAiming);
        _mover.SwitchRotatingWithInput(isAiming);
    }

    private void OnLookInput(Vector2 lookInput)
    {
        //characterCamera.Look(lookInput);
    }
}
