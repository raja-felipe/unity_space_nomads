using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GooStationData :  ShipStationData
{
    public int gooGain = 50;
    public override void onInteract(GameObject interactableObject)
    {
        PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources += gooGain;
        HudUiManager.HudManager.ShowGooChange(gooGain, true);
    }
    
    public override void UpdateDisplayString(int currentUses, float currentCooldown)
    {
        string interactKey = "E";
        displayString = string.Format("Press {0} to gain {1} Goo\nUsesLeft : {2}\nCooldown: {3}", interactKey, 
            gooGain, currentUses, Mathf.Max(useCooldown-currentCooldown, 0).ToString("0.00"));
    }
}
