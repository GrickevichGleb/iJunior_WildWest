using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharPlayer : Character
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharPlayerCamera _charPlayerCamera;
    [SerializeField] private Transform _aimCamPivot;

    private Vector3 _lookInputDirection;
    private Vector3 _moveInputDirection;

    private Transform _camTransform;
    
    public bool IsAiming { get; private set; } = false;

    public event Action<bool> AimSwitched;

    private void OnEnable()
    {
        _inputReader.MoveInput += OnMoveInput;

        _inputReader.Aiming += OnAiming;
        _inputReader.Attack += OnAttack;
    }

    private void Start()
    {
        Health.Death += OnDeath;
        
        OnAiming(false);
        
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        if(IsAiming)
            Mover.SetDesiredRotation(_aimCamPivot.forward);
        else 
            Mover.SetDesiredRotation(_camTransform.forward);
        
        Mover.SetDesiredMove(GetMoveDirection());
        
        PlayMovementAnimation();
    }

    private void OnDisable()
    {
        _inputReader.MoveInput -= OnMoveInput;

        _inputReader.Aiming -= OnAiming;
        _inputReader.Attack -= OnAttack;
    }
    
    public override void Reset()
    {
        base.Reset();

        CharAnimator.ResetState();

        Health.ResetCurrent();

        CharCollider.enabled = true;
        CharRigidbody.isKinematic = false;

        Mover.enabled = true;
        Attacker.enabled = true;
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 flattenedForward = 
            Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up).normalized;
        Vector3 flattenedRight = 
            Vector3.ProjectOnPlane(_camTransform.right, Vector3.up).normalized;

        Vector3 moveDirection = (flattenedForward * _moveInputDirection.z) + (flattenedRight * _moveInputDirection.x);

        return moveDirection.normalized;
    }

    private void PlayMovementAnimation()
    {
        Vector3 localMovement = transform.InverseTransformDirection(GetMoveDirection());

        CharAnimator.PlayMovement(localMovement);
    }

    private void OnMoveInput(Vector2 moveInput)
    {
        _moveInputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
    }

    private void OnAiming(bool isAiming)
    {
        IsAiming = isAiming;
        
        _charPlayerCamera.SwitchAimCamera(isAiming);
        CharAnimator.SetIsAiming(isAiming);
        
        AimSwitched?.Invoke(isAiming);
    }

    private void OnAttack()
    {
        if(IsAiming)
            Attacker.Attack();
    }

    private void OnDeath()
    {
        Mover.enabled = false;
        Attacker.enabled = false;

        CharAnimator.PlayDeath();
    }
}
