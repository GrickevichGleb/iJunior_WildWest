using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private const float FloatMargin = 0.01f;
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private Rigidbody _rigidbody;

    private Vector3 _moveDirection;

    private Vector3 _desiredMove;
    private Quaternion _desiredRotation;

    public bool IsMoving { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateIsMoving();
    }

    private void FixedUpdate()
    {
        RotateRbDesired();
        MoveRbDesired();
    }

    public void SetDesiredRotation(Vector3 direction)
    {
        if(direction == Vector3.zero)
            return;

        Vector3 flattened = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        
        Quaternion lookRotation = Quaternion.LookRotation(flattened, Vector3.up);
        _desiredRotation = lookRotation;
    }

    public void SetDesiredMove(Vector3 direction)
    {
        Vector3 moveVector = new Vector3(direction.x, 0f, direction.z).normalized;
        _desiredMove = moveVector;
    }

    private void RotateRbDesired()
    {
        // Quaternion toRotation = 
        //     Quaternion.RotateTowards(_rigidbody.rotation, _desiredRotation,
        //         _rotationSpeed * Time.fixedDeltaTime);
        //
        // _rigidbody.MoveRotation(toRotation);
        
        Quaternion deltaRotation = _desiredRotation * Quaternion.Inverse(_rigidbody.rotation);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
        
        if (angle > 180f)
            angle -= 360f;
        
        _rigidbody.angularVelocity = axis * (angle * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }

    private void MoveRbDesired()
    {
        Vector3 targetVelocity = _desiredMove * _moveSpeed;
        targetVelocity.y = _rigidbody.velocity.y;
        Vector3 velocityChange = targetVelocity - _rigidbody.velocity;
        
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void UpdateIsMoving()
    {
        if (_desiredMove.sqrMagnitude <= FloatMargin)
            IsMoving = false;
        else
            IsMoving = true;
    }
}
