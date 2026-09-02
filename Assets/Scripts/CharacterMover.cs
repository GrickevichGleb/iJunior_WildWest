using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private float _maxAcceleration = 120f;

    [SerializeField] private Transform _camFollowTransform;
    
    private Camera _camera;
    
    private Rigidbody _rigidbody;
    private CharacterAnimator _animator;

    private bool _isMoving;
    private Vector2 _moveInputVector;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<CharacterAnimator>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        CalculateRotationAndMovement();
    }

    private void FixedUpdate()
    {
        PerformMovement();
        
        _animator.PlayMovement(_moveInputVector);
    }

    public void Move(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude >= 0.01f)
            _isMoving = true;
        else
        {
            if(_isMoving == true)
                _rigidbody.velocity = Vector3.zero;
            _isMoving = false;
        }

        _moveInputVector = moveDirection;
    }

    private void CalculateRotationAndMovement()
    {
        if (_isMoving == false)
        {
            _moveDirection = Vector3.zero;
            return;
        }

        _moveDirection = GetMoveDirection(_moveInputVector);
    }

    private void PerformMovement()
    {
        if (_isMoving == false)
            return;
        
        Vector3 direction = new Vector3(_moveInputVector.x, 0f, _moveInputVector.y).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        
        RotateRb();
        MoveRb();
    }

    private void MoveRb()
    {
        Vector3 moveDirection = GetMoveDirection(_moveInputVector);
        
        
        _rigidbody.velocity = moveDirection * _moveSpeed;
    }

    private void RotateRb()
    {
        if (_moveDirection == Vector3.zero)
            return;
        
        Vector3 camForwardFlattened = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(camForwardFlattened, Vector3.up);

        Quaternion rotation =
            Quaternion.RotateTowards(_rigidbody.rotation, lookRotation, _rotationSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(rotation);
    }

    private Vector3 GetMoveDirection(Vector2 input)
    {
        Vector3 flattenedForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Vector3 flattenedRight = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;

        Vector3 moveDirection = (flattenedForward * input.y) + (flattenedRight * input.x);

        return moveDirection.normalized;
    }

    private Vector3 GetLookDirection(Transform dirTransform)
    {
        Vector3 flattenedForward = Vector3.ProjectOnPlane(dirTransform.forward, Vector3.up).normalized;

        return flattenedForward;
    }
}
