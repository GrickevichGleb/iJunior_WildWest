using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : Attacker
{
    //[SerializeField] private CharacterCamera _characterCamera;
    
    public override void Attack()
    {
        _weapon.TryAttack();
    }

    // private void AimWeapon()
    // {
    //     if (_characterCamera.TryGetAimPoint(out Vector3 aimPoint))
    //     {
    //         
    //     }
    // }
}
