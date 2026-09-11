using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : Attacker
{
    [SerializeField] private EnemyAnimator _enemyAnimator;

    private float _lastAttackTime;

    public bool IsAttacking { get; private set; }

    private void Start()
    {
        _enemyAnimator.OverrideAttackAnimation(_weapon);
    }

    public override void Attack()
    {
        if (_weapon.TryAttack())
        {
            _enemyAnimator.PlayAttack();
        }
    }
}
