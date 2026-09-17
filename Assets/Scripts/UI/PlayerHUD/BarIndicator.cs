using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarIndicator : MonoBehaviour
{
    private const float Tolerance = 0.0001f;
    
    [SerializeField] private StatValue _statValue;
    [SerializeField] private Image _fillerImage;
    [SerializeField] private float _changeSpeed = 0.8f;
    
    private Coroutine _displayChangesCoroutine;
    
    private void Start()
    {
        _statValue.Changed += OnValueChanged;
        
        OnValueChanged();
    }

    private void OnValueChanged()
    {
        if(_displayChangesCoroutine != null)
            StopCoroutine(_displayChangesCoroutine);

        _displayChangesCoroutine = StartCoroutine(ChangeFillAmountCoroutine());
    }
    
    private IEnumerator ChangeFillAmountCoroutine()
    {
        float targetAmount = Convert.ToSingle(_statValue.Current) / Convert.ToSingle(_statValue.Max);

        while (Math.Abs(_fillerImage.fillAmount - targetAmount) > Tolerance)
        {
            _fillerImage.fillAmount = 
                Mathf.MoveTowards(_fillerImage.fillAmount, targetAmount, _changeSpeed * Time.deltaTime);
            
            yield return null;
        }
    }
}
