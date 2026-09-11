using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private CharacterPlayer _player;
    [SerializeField] private Image _aimReticule;

    private void OnEnable()
    {
        _player.AimSwitched += OnAimSwitched;
    }

    private void OnDisable()
    {
        _player.AimSwitched -= OnAimSwitched;
    }

    private void OnAimSwitched(bool isAiming)
    {
        _aimReticule.enabled = isAiming;
    }
}
