using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimator : MonoBehaviour
{
    protected const string IsMovingPar = "IsMoving";
    protected const string MoveXPar = "MoveX";
    protected const string MoveZPar = "MoveZ";
    protected const string IsAimingPar = "IsAiming";
    protected const string AttackTrig = "Attack";
    protected const string DeathTrig = "Death";
    
    [SerializeField] protected Animator _animator;

    public void ResetPrams()
    {
        _animator.enabled = true;
        
        _animator.SetBool(IsMovingPar, false);
    }
    
    public void OverrideAttackAnimation(Weapon weapon)
    {
        weapon.SetupAnimator(_animator);
    }
    
    public virtual void SetIsMoving(bool isMoving)
    {
        _animator.SetBool(IsMovingPar, isMoving);
    }

    public virtual void PlayMovement(Vector3 moveVector)
    {
        Vector3 normalizedMovement = moveVector.normalized;

        if (moveVector != Vector3.zero)
            SetIsMoving(true);
        else
            SetIsMoving(false);
        
        _animator.SetFloat(MoveXPar, normalizedMovement.x);
        _animator.SetFloat(MoveZPar, normalizedMovement.z);
    }

    public virtual void SetIsAiming(bool isAiming)
    {
        _animator.SetBool(IsAimingPar, isAiming);
    }

    public virtual void PlayAttack()
    {
        _animator.SetTrigger(AttackTrig);
    }

    public virtual void PlayDeath()
    {
        _animator.SetTrigger(DeathTrig);
    }
}
