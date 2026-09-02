using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharacterMover _mover;
    [SerializeField] private LookAroundCamera lookAroundCamera;
    
    private void OnEnable()
    {
        _inputReader.MoveInput += OnMoveInput;
        _inputReader.LookInput += OnLookInput;
    }

    private void OnDisable()
    {
        _inputReader.MoveInput -= OnMoveInput;
        _inputReader.LookInput -= OnLookInput;
    }

    private void OnMoveInput(Vector2 moveInput)
    {
        _mover.Move(moveInput);
    }

    private void OnLookInput(Vector2 lookInput)
    {
        lookAroundCamera.Look(lookInput);
    }
}
