using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _isMoving = false;
    
    public void PlayMovement(Vector2 moveInput)
    {
        SetIsMoving(moveInput);

        if (_isMoving == false)
            return;
        
        _animator.SetBool("IsMoving", true);
        
        _animator.SetFloat("MoveX", moveInput.x);
        _animator.SetFloat("MoveZ", moveInput.y);

        _isMoving = true;
    }

    private void SetIsMoving(Vector2 moveInput)
    {
        if (moveInput == Vector2.zero)
        {
            if(_isMoving == true)
                _animator.SetBool("IsMoving", false);

            _isMoving = false;
        }
        else
        {
            if(_isMoving == false)
                _animator.SetBool("IsMoving", true);

            _isMoving = true;
        }
    }

}
