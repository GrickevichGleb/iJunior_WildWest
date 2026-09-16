using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharPlayerCamera : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _aimCameraPivot;
    
    [SerializeField] private CinemachineInputProvider _lookAroundInputProvider;
    [SerializeField] private CinemachineFreeLook _cmLookAroundCam;
    [SerializeField] private CinemachineVirtualCamera _cmAimCamera;

    [SerializeField] private float _maxRotationAngle = 42f;
    [SerializeField] private float _minRotationAngle = -45f;
    
    [SerializeField] private Rig _aimRig;
    [SerializeField] private Transform _aimTarget;
    [SerializeField] private LayerMask _aimLayerMask;

    private Transform _mainCameraTransform;

    private Vector3 _aimRotationDeltaVector = new Vector3();
    
    private bool _isAiming = false;
    private float _lastRotationY = 0f;
    
    private void OnEnable()
    {
        _inputReader.LookInput += OnLookInput;
    }

    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;

        _lastRotationY = _aimCameraPivot.rotation.eulerAngles.y;
    }

    private void Update()
    {
        RotateAimCamera();
        
        AdjustAimTargetPosition();
    }

    private void OnDisable()
    {
        _inputReader.LookInput -= OnLookInput;
    }

    public void SwitchAimCamera(bool isAiming)
    {
        if (isAiming == true)
            EnableAimCamera();
        else
            EnableLookAroundCam();
        
        //_animator.SetIsAiming(isAiming);
    }

    private void RotateAimCamera()
    {
        if (_isAiming == false)
            return;
        
        Vector3 localEulerAngles = _aimCameraPivot.localRotation.eulerAngles;
        float currentLocalX = localEulerAngles.x;

        if ( currentLocalX > 180f)
             currentLocalX -= 360;
        
        float newAngleX = currentLocalX + _aimRotationDeltaVector.x;
        newAngleX = Mathf.Clamp(newAngleX, _minRotationAngle, _maxRotationAngle);
        localEulerAngles.x = newAngleX;
        _aimCameraPivot.localRotation = Quaternion.Euler(localEulerAngles);


        Vector3 globalEulerAngles = _aimCameraPivot.rotation.eulerAngles;
        float newAngleY = _lastRotationY + _aimRotationDeltaVector.y;
        globalEulerAngles.y = newAngleY;
        _aimCameraPivot.rotation = Quaternion.Euler(globalEulerAngles);

        _lastRotationY = newAngleY;
    }

    private void EnableAimCamera()
    {
        _isAiming = true;

        _aimCameraPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _lookAroundInputProvider.enabled = false;
        _cmLookAroundCam.gameObject.SetActive(false);
        _cmAimCamera.gameObject.SetActive(true);
        
        _aimRig.weight = 1f;
    }

    private void EnableLookAroundCam()
    {
        _isAiming = false;

        SnapFreeLookBehindPlayer();
        
        _aimCameraPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
        
        _lookAroundInputProvider.enabled = true;
        _cmLookAroundCam.gameObject.SetActive(true);
        _cmAimCamera.gameObject.SetActive(false);
        
        _aimRig.weight = 0f;
    }

    private void SnapFreeLookBehindPlayer()
    {
        _cmLookAroundCam.m_XAxis.Value = transform.eulerAngles.y;

        float pivotLocalX = _aimCameraPivot.localRotation.eulerAngles.x;

        if (pivotLocalX > 180f)
            pivotLocalX -= 360;

        float yAxisValue = Mathf.InverseLerp(_minRotationAngle, _maxRotationAngle, pivotLocalX);
        _cmLookAroundCam.m_YAxis.Value = yAxisValue;
        
        _cmLookAroundCam.PreviousStateIsValid = false;
    }
    
    private void AdjustAimTargetPosition()
    {
        if(_isAiming == true)
        {
            if (Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, 
                    out RaycastHit hit, 100f, _aimLayerMask))
            {
                _aimTarget.transform.position = hit.point;
                return;
            }
        }
        
        _aimTarget.transform.position = _aimCameraPivot.position + _aimCameraPivot.forward * 3f;
    }
    
    private void OnLookInput(Vector2 lookInput)
    {
        _aimRotationDeltaVector = new Vector3(-lookInput.y, lookInput.x, 0f);
    }
}
