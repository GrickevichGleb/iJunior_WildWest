using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : Attacker
{
    private CharAnimator _charAnimator;
    
    private float _lastAttackTime;

    public bool IsAttacking { get; private set; }

    private void Start()
    {
        _charAnimator = GetComponent<CharAnimator>();
        _charAnimator.OverrideAttackAnimation(_weapon);
    }

    public override void Attack()
    {
        if (_weapon.TryAttack())
        {
            _charAnimator.PlayAttack();
        }
    }
}
