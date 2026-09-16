using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEnemy : Character
{
    [SerializeField] private Transform _attackTarget;
    [SerializeField] private float _stoppingDistance;

    private Vector3 _moveDirection;
    private Vector3 _lookDirection;
    
    private bool _isDead = false;

    
    private void Start()
    {
        Health.Death += OnDeath;
    }

    private void Update()
    {
        SelectBehaviour();
        
        PlayMovementAnimation();
    }

    public void Initialize(Vector3 initPosition, Transform target)
    {
        transform.position = initPosition;
        _attackTarget = target;
    }

    public override void Reset()
    {
        base.Reset();

        CharAnimator.ResetPrams();
        Health.ResetCurrent();
        
        _isDead = false;
        
        CharCollider.enabled = true;
        CharRigidbody.isKinematic = false;

        Mover.enabled = true;
        Attacker.enabled = true;
    }
    
    private void SelectBehaviour()
    {
        if (_isDead == true)
            return;
        
        if(Attacker.CheckCanAttack(_attackTarget.transform))
            AttackBehaviour();

        ChaseBehaviour();
    }

    private void AttackBehaviour()
    {
        Attacker.Attack();
    }

    private void ChaseBehaviour()
    {
        _moveDirection = (_attackTarget.position - transform.position).normalized;
        
        if (CheckReachedDestination() == true)
            _moveDirection = Vector3.zero;
        
        Mover.SetDesiredMove(_moveDirection);
        Mover.SetDesiredRotation(_moveDirection);
    }

    private bool CheckReachedDestination()
    {
        Vector3 dirVector = _attackTarget.position - transform.position;
        
        if (dirVector.sqrMagnitude <= Mathf.Pow(_stoppingDistance, 2))
            return true;

        return false;
    }
    
    
    private IEnumerator ReleaseDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Release();
    }

    private void OnDeath()
    {
        _isDead = true;

        Mover.enabled = false;
        Attacker.enabled = false;

        CharRigidbody.isKinematic = true;
        CharCollider.enabled = false;
        
        CharAnimator.PlayDeath();

        StartCoroutine(ReleaseDelayed(2f));
    }
    
    private void PlayMovementAnimation()
    {
        Vector3 localMovement = transform.InverseTransformDirection(_moveDirection);

        CharAnimator.PlayMovement(localMovement);
    }
}
