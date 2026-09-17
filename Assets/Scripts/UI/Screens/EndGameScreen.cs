using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameScreen : ScreenPanel
{
    [SerializeField] private TMP_Text _titleText;
    
    public event Action RestartButtonClicked;

    public void SetTitleText(string text)
    {
        _titleText.text = text;
    }
    
    public override void Close()
    {
        CanvasGroup.alpha = 0f;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }

    public override void Open()
    {
        CanvasGroup.alpha = 1f;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }
    
    protected override void OnButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }
}
