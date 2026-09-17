using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : StatValue
{
    private bool _isDead = false;
    
    public event Action Death;

    private void Awake()
    {
        ResetCurrent();
    }

    public void TakeDamage(int damage)
    {
        if(_isDead == true)
            return;

        Current -= damage;
        Current = Mathf.Clamp(Current, 0, Max);
        
        if(Current <= 0)
            Die();

        InvokeChanged();
    }

    public void ResetCurrent()
    {
        _isDead = false;
        Current = Max;
        
        InvokeChanged();
    }

    private void Die()
    {
        _isDead = true;
        Current = 0;
        
        Death?.Invoke();
    }
}
