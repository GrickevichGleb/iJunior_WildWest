using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    [SerializeField] private BoxCollider _damageCollider;
    
    [SerializeField] private float _delayBeforeActiveDamage;
    [SerializeField] private float _activeDamageDuration;

    private Coroutine _activeDamageCoroutine;
    
    private bool _isDamageActive;
    private bool _hasDealtDamage;
    
    private void OnTriggerEnter(Collider other)
    {
        if(_isDamageActive == false)
            return;

        if (other.gameObject == gameObject)
            return;

        if (other.TryGetComponent(out Health targetHealth))
        {
            targetHealth.TakeDamage(Damage);
        }
    }

    protected override void PerformAttack()
    {
        _hasDealtDamage = false;
        
        if(_activeDamageCoroutine != null)
            StopCoroutine(_activeDamageCoroutine);
        _activeDamageCoroutine =
            StartCoroutine(ActivateDamageCoroutine(_delayBeforeActiveDamage, _activeDamageDuration));
    }
    
    private IEnumerator ActivateDamageCoroutine(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        _isDamageActive = true;
        _damageCollider.enabled = true;

        yield return new WaitForSeconds(duration);

        _isDamageActive = false;
        _damageCollider.enabled = false;
    }
}
