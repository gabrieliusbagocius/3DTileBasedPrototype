using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Image foregroundImage = null;
    [SerializeField]
    private Image backgroundImage = null;
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;
    private void Awake()
    {
        GetComponentInParent<BaseUnit>().OnHealthPctChanged += HandleHealthChange;
    }

    public void HandleHealthChange(float pct)
    {
        foregroundImage.fillAmount = pct;
    }

    public void DisableHealthBars()
    {
        backgroundImage.enabled = false;
        foregroundImage.enabled = false;
    }

}
