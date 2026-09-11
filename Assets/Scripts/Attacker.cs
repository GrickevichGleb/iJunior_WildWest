using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;

    public virtual void Attack() { }
    
    
}
