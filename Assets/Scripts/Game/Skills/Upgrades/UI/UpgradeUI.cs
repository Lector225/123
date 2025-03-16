using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Image icon;

    private Action applyAction;

    public void Setup(string title, Sprite icon, Action onApply)
    {
        this.title.text = title;
        this.icon.sprite = icon;
        this.applyAction = onApply;
    }

    public void Apply()
    {
        applyAction?.Invoke();
    }
}
