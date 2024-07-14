using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrenadeStationData :  ShipStationData
{
    public int grenadeGainCount = 1;
    // Start is called before the first frame update
    public override void onInteract(GameObject interactableObject)
    {
        PlayerGunScript.currentGunScript.grenadeCount += grenadeGainCount;
    }

    public override void UpdateDisplayString(int currentUses, float currentCooldown)
    {
        string interactKey = "E";
        displayString = string.Format("Press {0} to gain {1} Grenades\nUses Left: {2}\nCooldown: {3}", interactKey, 
            grenadeGainCount, currentUses, Mathf.Max(useCooldown-currentCooldown, 0).ToString("0.00"));
    }
}
