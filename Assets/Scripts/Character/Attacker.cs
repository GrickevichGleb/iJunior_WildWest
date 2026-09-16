using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    private const float SightCheckSphereRadius = 0.3f;
    
    [SerializeField] protected Weapon _weapon;

    public virtual void Attack() { }

    public virtual bool CheckCanAttack(Transform target)
    {
        if (_weapon.IsRangedWeapon == false)
        {
            if (CheckDistance(target, _weapon.AttackRange))
                return true;
        }
        else if (_weapon.IsRangedWeapon == true)
        {
            if (CheckDistance(target, _weapon.AttackRange) &&
                CheckDirectSight(target, _weapon.AttackRange))
                return true;
        }

        return false;
    }

    private bool CheckDistance(Transform target, float maxDistance)
    {
        Vector3 dirVector = target.position - _weapon.transform.position;

        if (dirVector.sqrMagnitude <= Mathf.Pow(maxDistance, 2))
            return true;

        return false;
    }

    private bool CheckDirectSight(Transform target, float maxDistance)
    {
        Vector3 dirVector = target.position - _weapon.transform.position;

        RaycastHit[] hits;

        hits = Physics.SphereCastAll(_weapon.transform.position,
            SightCheckSphereRadius, dirVector, maxDistance);
        if (hits.Length == 0)
            return false;

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == target.gameObject)
                return true;
        }

        return false;
    }
}
