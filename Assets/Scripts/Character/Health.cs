using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private bool _isDead = false;
    
    public event Action Changed;
    public event Action Death;

    [field: SerializeField] public int Max { get; private set; }
    public int Current { get; private set; }

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
        
        Changed?.Invoke();
    }

    public void ResetCurrent()
    {
        Current = Max;
    }

    private void Die()
    {
        _isDead = true;
        Current = 0;
        
        Death?.Invoke();
    }
}
