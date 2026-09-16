using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = _targetTransform.position + _offset;
    }
}
