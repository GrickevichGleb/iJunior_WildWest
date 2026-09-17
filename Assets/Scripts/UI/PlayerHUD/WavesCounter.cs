using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WavesCounter : MonoBehaviour
{
    [SerializeField] private StatValue _statValue;
    [SerializeField] private TMP_Text _textField;

    private void Start()
    {
        _statValue.Changed += OnValueChanged;
        
        OnValueChanged();
    }

    private void OnValueChanged()
    {
        _textField.text = $"Wave: {_statValue.Current} / {_statValue.Max}";
    }
}
