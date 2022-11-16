using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHero : BaseUnit
{
    [HideInInspector]
    public int killCount = 0;
    public event System.Action<int> OnKCChanged = delegate { };

    internal void Death()
    {
        Alive = false;
        OccupiedTile.RemoveUnit(this);
        GetComponent<HeroVisualsBehaviour>().playerMeshRenderer.enabled = false;
        GetComponent<HealthBar>().DisableHealthBars();
    }

    public void IncrementKC()
    {
        killCount += 1;
        OnKCChanged(killCount);
    }

}
