using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipStationData : InteractableData
{
    public float maxHealth = 200;
    public int maxUses = 3;
    public float useRechargeTime= 30;
    public float useCooldown = 2;
    public string displayString;
    public abstract void UpdateDisplayString(int currentUses, float currentCooldown);
}
