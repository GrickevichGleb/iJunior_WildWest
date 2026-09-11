using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const float FloatMargin = 0.01f;
    
    private const string IsMovingPar = "IsMoving";
    private const string DeathPar = "Death";
    private const string AttackPar = "Attack";
    
    [SerializeField] private Animator _animator;

    private bool _isMoving = false;

    public void OverrideAttackAnimation(Weapon weapon)
    {
        weapon.SetupAnimator(_animator);
    }
    
    public void SetIsMoving(Vector3 moveVector)
    {
        if (moveVector.sqrMagnitude >= FloatMargin)
        {
            if(_isMoving == false)
                _animator.SetBool(IsMovingPar, true);
            
            _isMoving = true;
        }
        else
        {
            if(_isMoving == true)
                _animator.SetBool(IsMovingPar, false);
            
            _isMoving = false;
        }
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(AttackPar);
    }

    public void PlayDeath()
    {
        _animator.SetTrigger(DeathPar);
    }

    public void ResetParams()
    {
        _animator.enabled = true;
        
        _animator.SetBool(IsMovingPar, false);
        _animator.Update(0f);
    }
}
