using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Spawnable
{
    protected Health Health;
    protected Mover Mover;
    protected Attacker Attacker;
    protected CharAnimator CharAnimator;

    protected Rigidbody CharRigidbody;
    protected Collider CharCollider;
    
    private void Awake()
    {
        Health = GetComponent<Health>();
        Mover = GetComponent<Mover>();
        Attacker = GetComponent<Attacker>();
        CharAnimator = GetComponent<CharAnimator>();

        CharRigidbody = GetComponent<Rigidbody>();
        CharCollider = GetComponent<Collider>();
    }
}
