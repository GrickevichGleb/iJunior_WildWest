using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private float _rotationMultiplyerY = 350f;

    [SerializeField] private Transform _camFollowTransform;
    
    private Camera _camera;
    
    private Rigidbody _rigidbody;
    private CharacterAnimator _animator;

    private bool _isMoving;
    private bool _isRotatingWithCamera;
    private bool _isRotatingWithInput;

    private float _rotationDegree;
    private Vector3 _desiredRotation;

    private Vector2 _moveInputVector;
    private Vector2 _lookInputVector;
    
    private Vector3 _moveDirection;
    private Quaternion _rotationToInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<CharacterAnimator>();
    }

    private void OnEnable()
    {
        _inputReader.LookInput += OnLookInput;
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
        
        RotateRbWithInput();

        _animator.PlayMovement(_moveInputVector);
    }

    private void OnDisable()
    {
        _inputReader.LookInput -= OnLookInput;
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

    public void SwitchRotatingWithInput(bool isRotating)
    {
        _isRotatingWithInput = isRotating;
        
    }
    
    private void RotateRbWithInput()
    {
        if (_isRotatingWithInput == false)
            return;
        
        Vector3 rotationSpeedVector = new Vector3(0f, _rotationSpeed / 2f, 0f);
        Vector3 rotationVector = rotationSpeedVector * _lookInputVector.x;
        Quaternion rotationDelta = Quaternion.Euler(rotationVector * Time.fixedDeltaTime);
        
        _rigidbody.MoveRotation(_rigidbody.rotation * rotationDelta);
        
        _lookInputVector = Vector2.zero;
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
        if (_moveDirection == Vector3.zero || _isRotatingWithInput == true)
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

    private void OnLookInput(Vector2 lookInput)
    {
        if(_isRotatingWithInput)
            _lookInputVector += lookInput;
    }
}
