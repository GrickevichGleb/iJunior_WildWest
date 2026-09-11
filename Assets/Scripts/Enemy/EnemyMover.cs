using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private EnemyAnimator _animator;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 360f;

    [SerializeField] private float _reachDistance = 1.2f;
    
    private Transform _moveToTarget;
    
    private Vector3 _moveDirection;

    private Vector3 _desiredDirection;
    private Quaternion _desiredRotation;

    public bool HasTarget { get; private set; }
    public bool IsTargetReached { get; private set; }

    private void Update()
    {
        CheckReachedTarget();
        
        SetMoveDirection();
        SetMoveRotation();
        
        _animator.SetIsMoving(_moveDirection);
    }

    private void FixedUpdate()
    {
        RotateRbDesired();
        MoveRbDesired();
    }

    public void SetTarget(Transform target)
    {
        _moveToTarget = target;
        HasTarget = true;
    }
    
    private void SetMoveDirection()
    {
        if (IsTargetReached == false)
        {
            _moveDirection = (_moveToTarget.position - transform.position);
            _moveDirection.y = 0f;
            
            _moveDirection.Normalize();
        }
        else if (IsTargetReached == true)
        {
            _moveDirection = Vector3.zero;
        }
    }

    private void MoveRbDesired()
    {
        Vector3 targetVelocity = _moveDirection * _moveSpeed;
        targetVelocity.y = _rigidbody.velocity.y;
        Vector3 velocityChange = targetVelocity - _rigidbody.velocity;
        
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void SetMoveRotation()
    {
        Vector3 directionToTarget = (_moveToTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        _desiredRotation = lookRotation;
    }

    private void RotateRbDesired()
    {
        Quaternion toRotation = 
            Quaternion.RotateTowards(_rigidbody.rotation, _desiredRotation, _rotationSpeed * Time.fixedDeltaTime);
        
        _rigidbody.MoveRotation(toRotation);
    }

    private void CheckReachedTarget()
    {
        float distance = (transform.position - _moveToTarget.position).magnitude;

        if (distance <= _reachDistance)
            IsTargetReached = true;
        else
            IsTargetReached = false;
    }
}
