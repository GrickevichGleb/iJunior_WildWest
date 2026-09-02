using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundCamera : MonoBehaviour
{
    [SerializeField] private Transform _camFollowTransform;

    [SerializeField] private float _horizontalRotationSpeed = 360f;
    [SerializeField] private float _verticalRotationSpeed = 240f;

    private Rigidbody _rigidbody;
    private Transform _characterTransform;
    
    private Vector3 _rotationVector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _characterTransform = transform;
    }
    
    private void Update()
    {
        RotateLook();
    }

    public void Look(Vector2 lookDelta)
    {
        _rotationVector = new Vector3(-lookDelta.y, lookDelta.x, 0f);
    }

    private void RotateLook()
    {
        if (_rotationVector.sqrMagnitude <= 0.1f)
            return;
        
        float horizontalLookSpeed = _horizontalRotationSpeed * Time.deltaTime;
        float verticalLookSpeed = _verticalRotationSpeed * Time.deltaTime;
        
        Quaternion charRotationY =
            Quaternion.AngleAxis(_camFollowTransform.localEulerAngles.y + _rotationVector.y * horizontalLookSpeed, Vector3.up);
        Quaternion camRotationX = 
            Quaternion.AngleAxis(_camFollowTransform.localEulerAngles.x + _rotationVector.x * verticalLookSpeed, Vector3.right);
        
        _camFollowTransform.localRotation = charRotationY * camRotationX;
    }
}
