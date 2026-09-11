using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Spawnable
{
    [SerializeField] private CharacterPlayer _player;

    private EnemyAnimator _animator;
    private EnemyMover _mover;
    private EnemyAttacker _attacker;
    private Health _health;

    private bool _isDead = false;
    
    private void Awake()
    {
        _animator = GetComponent<EnemyAnimator>();
        _mover = GetComponent<EnemyMover>();
        _attacker = GetComponent<EnemyAttacker>();
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _mover.SetTarget(_player.transform);

        _health.Death += OnDeath;
    }

    private void Update()
    {
        SelectBehaviour();
    }

    public void Initialize(CharacterPlayer player)
    {
        _player = player;
        
    }
    
    
    
    public override void Reset()
    {
        base.Reset();

        _animator.ResetParams();
        _health.ResetCurrent();
        
        _isDead = false;
        
        _mover.enabled = true;
        _attacker.enabled = true;
    }

    private void SelectBehaviour()
    {
        if (_isDead == true)
            return;
        
        if(_mover.IsTargetReached == false)
            ChaseBehaviour();
        else if(_mover.IsTargetReached == true)
            AttackBehaviour();
    }

    private void ChaseBehaviour()
    {
        if(_mover.HasTarget == false)
            _mover.SetTarget(_player.transform);
    }

    private void AttackBehaviour()
    {
        if(_attacker.IsAttacking == false)
            _attacker.Attack();
    }

    private void OnDeath()
    {
        _isDead = true;

        _mover.enabled = false;
        _attacker.enabled = false;
        
        _animator.PlayDeath();
        _animator.enabled = false;
    }
}
