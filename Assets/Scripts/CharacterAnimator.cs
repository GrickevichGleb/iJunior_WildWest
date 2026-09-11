using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private const string IsMovingPar = "IsMoving";
    private const string MoveXPar = "MoveX";
    private const string MoveZPar = "MoveZ";
    private const string IsAimingPar = "IsAiming";
    
    [SerializeField] private Animator _animator;

    private bool _isMoving = false;
    
    public void PlayMovement(Vector2 moveInput)
    {
        SetIsMoving(moveInput);

        if (_isMoving == false)
            return;
        
        _animator.SetBool(IsMovingPar, true);
        
        _animator.SetFloat(MoveXPar, moveInput.x);
        _animator.SetFloat(MoveZPar, moveInput.y);

        _isMoving = true;
    }

    public void SetIsAiming(bool isAiming)
    {
        _animator.SetBool(IsAimingPar, isAiming);
    }

    private void SetIsMoving(Vector2 moveInput)
    {
        if (moveInput == Vector2.zero)
        {
            if(_isMoving == true)
                _animator.SetBool(IsMovingPar, false);

            _isMoving = false;
        }
        else
        {
            if(_isMoving == false)
                _animator.SetBool(IsMovingPar, true);

            _isMoving = true;
        }
    }
    
    

}
