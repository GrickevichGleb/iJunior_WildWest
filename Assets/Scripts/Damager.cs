using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private int _damage;
    
    public void DealDamage(Health health)
    {
        health.TakeDamage(_damage);
    }
}
