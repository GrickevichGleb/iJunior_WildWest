using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharacterCamera _characterCamera;
    
    [SerializeField] private CharacterMover _mover;
    [SerializeField] private PlayerAttacker _attacker;

    public bool IsAiming { get; private set; }

    public event Action<bool> AimSwitched;
    
    private void OnEnable()
    {
        _inputReader.MoveInput += OnMoveInput;
        _inputReader.LookInput += OnLookInput;

        _inputReader.Aiming += OnAiming;
        _inputReader.Attack += OnAttack;
    }

    private void Start()
    {
        OnAiming(false);
    }

    private void OnDisable()
    {
        _inputReader.MoveInput -= OnMoveInput;
        _inputReader.LookInput -= OnLookInput;
        
        _inputReader.Aiming -= OnAiming;
        _inputReader.Attack -= OnAttack;
    }

    private void OnMoveInput(Vector2 moveInput)
    {
        _mover.Move(moveInput);
    }

    private void OnLookInput(Vector2 lookInput)
    {
        //characterCamera.Look(lookInput);
    }

    private void OnAiming(bool isAiming)
    {
        _mover.SwitchRotatingWithInput(isAiming);
        _characterCamera.SwitchAimCamera(isAiming);

        IsAiming = isAiming;
        
        AimSwitched?.Invoke(IsAiming);
    }

    private void OnAttack()
    {
        if(IsAiming)
            _attacker.Attack();
    }
}
