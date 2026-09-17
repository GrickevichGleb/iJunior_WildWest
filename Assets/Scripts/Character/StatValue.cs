using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatValue : MonoBehaviour
{
    [field: SerializeField] public int Min { get; protected set; }
    [field: SerializeField] public int Max { get; protected set; }
    [field: SerializeField] public int Current { get; protected set; }

    public event Action Changed;

    protected virtual void InvokeChanged()
    {
        Changed?.Invoke();
    }
}
