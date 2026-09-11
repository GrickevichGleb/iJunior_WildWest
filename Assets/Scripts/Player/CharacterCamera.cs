using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class CharacterCamera : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _aimLayerMask;
    
    [SerializeField] private CinemachineInputProvider _lookAroundInputProvider;
    [SerializeField] private CinemachineFreeLook _cmLookAroundCam;
    [SerializeField] private CinemachineVirtualCamera _cmAimCamera;
    
    [SerializeField] private Transform _aimCameraPivot;
    [SerializeField] private float _maxRotationAngle = 42f;
    [SerializeField] private float _minRotationAngle = -32f;
    
    [SerializeField] private Rig _aimRig;
    [SerializeField] private Transform _aimTarget;
    
    [SerializeField] private CharacterAnimator _animator;

    private Transform _mainCameraTransform;
    
    private bool _isAiming = false;
    private Vector3 _lookMovement = new Vector3();
    
    private void OnEnable()
    {
        _inputReader.LookInput += OnLookInput;
    }

    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_isAiming == true)
        {
            CamPivotVertical();
        }

        AdjustAimTargetPosition();
    }


    private void OnDisable()
    {
        _inputReader.LookInput -= OnLookInput;
    }

    public bool TryGetAimPoint(out Vector3 aimPoint)
    {
        aimPoint = Vector3.zero;
        
        if (_isAiming == false)
            return false;

        aimPoint = _aimTarget.position;
        return true;
    }
    
    public void SwitchAimCamera(bool isActive)
    {
        if (isActive == true)
            EnableAimCamera();
        else
            EnableLookAroundCam();
        
        _animator.SetIsAiming(isActive);
    }

    private void CamPivotVertical()
    {
        Vector3 eulerAngles = _aimCameraPivot.localRotation.eulerAngles;
        float curAngle = eulerAngles.x;
        
        if (curAngle > 180f)
            curAngle -= 360;
        
        float newAngleX = curAngle + _lookMovement.x;
        newAngleX = Mathf.Clamp(newAngleX, _minRotationAngle, _maxRotationAngle);

        eulerAngles.x = newAngleX;    
        _aimCameraPivot.localRotation = Quaternion.Euler(eulerAngles);
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

    private void EnableAimCamera()
    {
        _isAiming = true;

        _aimCameraPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _lookAroundInputProvider.enabled = false;
        _cmLookAroundCam.gameObject.SetActive(false);
        _cmAimCamera.gameObject.SetActive(true);
        
        _aimRig.weight = 1f;
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
    
    private void OnLookInput(Vector2 lookInput)
    {
        _lookMovement = new Vector3(-lookInput.y, lookInput.x, 0f);
    }
}
