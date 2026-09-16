using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private Transform _firingPoint;
    [SerializeField] private float _shotMaxDistance = 100f;
    [SerializeField] private ParticleSystem _shotEffect;

    protected override void PerformAttack()
    {
        _shotEffect.Play();
        Shoot();
    }

    private void Shoot()
    {
        //Debug.DrawRay(_firingPoint.position, _firingPoint.forward * 50f, Color.red, 1f);

        if (Physics.Raycast(_firingPoint.position, _firingPoint.forward, out RaycastHit hit, _shotMaxDistance))
        {
            if(hit.collider.TryGetComponent(out Health health))
            {
                health.TakeDamage(Damage);
            }
        }
    }
}
