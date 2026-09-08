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
    
    private Vector2 _moveInputVector;
    private Vector2 _lookInputVector;
    
    private Quaternion _desiredRotation;
    private Vector3 _desiredMoveDirection;

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
        UpdateRotation();
        UpdateMoveDesired();
    }

    private void FixedUpdate()
    {
        RotateRbDesired();
        MoveRbDesired();
     
        PlayMovementAnimation();
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
        _lookInputVector = Vector2.zero;
        
        _isRotatingWithInput = isRotating;
        _isRotatingWithCamera = !isRotating;
        //
        //
        // if(isRotating)
        //     SetRotationWithCamera();
    }
    
    private void UpdateRotation()
    {
        if(_isRotatingWithInput)
            SetRotationWithInput();
        
        if(_isRotatingWithCamera)
            SetRotationWithCamera();
    }

    private void SetRotationWithInput()
    {
        Vector3 rotationSpeedVector = new Vector3(0f, _rotationSpeed / 2f, 0f);
        Vector3 rotationVector = rotationSpeedVector * _lookInputVector.x;
        Quaternion rotationDelta = Quaternion.Euler(rotationVector * Time.fixedDeltaTime);

        _desiredRotation = _rigidbody.rotation * rotationDelta;
    }

    private void SetRotationWithCamera()
    {
        Vector3 camForwardFlattened = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(camForwardFlattened, Vector3.up);

        _desiredRotation = lookRotation;
    }

    private void RotateRbDesired()
    {
        if (_isRotatingWithCamera == true && _isMoving == false)
            return;
        
        if (_isRotatingWithInput)
        {
            Quaternion toRotation = 
                Quaternion.RotateTowards(_rigidbody.rotation, _desiredRotation, _rotationSpeed * Time.fixedDeltaTime);
        
            _rigidbody.MoveRotation(toRotation);
            
            _lookInputVector = Vector2.zero;
        }
        else if (_isRotatingWithCamera)
        {
            Quaternion deltaRotation = _desiredRotation * Quaternion.Inverse(_rigidbody.rotation);
            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f)
                angle -= 360f;

            _rigidbody.angularVelocity = axis * (angle * Mathf.Deg2Rad / Time.fixedDeltaTime);
        }
    }

    private void UpdateMoveDesired()
    {
        Vector3 moveDirection = GetMoveDirection(_moveInputVector);
        moveDirection.Normalize();

        _desiredMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);
    }

    private Vector3 GetMoveDirection(Vector2 input)
    {
        Vector3 flattenedForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Vector3 flattenedRight = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;

        Vector3 moveDirection = (flattenedForward * input.y) + (flattenedRight * input.x);

        return moveDirection.normalized;
    }

    private void MoveRbDesired()
    {
        Vector3 targetVelocity = _desiredMoveDirection * _moveSpeed;
        targetVelocity.y = _rigidbody.velocity.y;
        Vector3 velocityChange = targetVelocity - _rigidbody.velocity;
        
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void PlayMovementAnimation()
    {
        Vector3 localMovement = transform.InverseTransformDirection(GetMoveDirection(_moveInputVector));
        Vector2 animMoveVector = new Vector2(localMovement.x, localMovement.z);
        
        _animator.PlayMovement(animMoveVector);
    }

    private void OnLookInput(Vector2 lookInput)
    {
        if(_isRotatingWithInput)
            _lookInputVector += lookInput;

        //_lookInputVector = lookInput;
    }
}
