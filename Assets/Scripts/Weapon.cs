using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected AnimatorOverrideController _animatorOverride;
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int AttackInterval { get; private set; }

    public bool IsReadyToAttack { get; private set; }

    private float _lastAttackTime;

    public virtual void SetupAnimator(Animator animator)
    {
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (_animatorOverride != null)
        {
            animator.runtimeAnimatorController = _animatorOverride;
        }
        else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
    }
    
    public bool TryAttack()
    {
        UpdateReadiness();
        
        if (IsReadyToAttack == true)
        {
            PerformAttack();

            _lastAttackTime = Time.time;
            return true;
        }

        return false;
    }

    protected virtual void PerformAttack()
    {
        
    }
    
    private void UpdateReadiness()
    {
        if (Time.time - _lastAttackTime >= AttackInterval)
            IsReadyToAttack = true;
        else
            IsReadyToAttack = false;
    }
}
