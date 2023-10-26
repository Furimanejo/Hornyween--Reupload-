using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PeriodicWeapon : EquipmentBase
{
    float periodTimer = 0f;
    
    protected virtual void Update()
    {
        var data = CurrentLevelData;
        if (data == null)
            return;

        periodTimer += Time.deltaTime;
        if (periodTimer >= data.cooldown)
        {
            periodTimer = 0f;
            Trigger();
        }
    }

    protected abstract void Trigger();
}