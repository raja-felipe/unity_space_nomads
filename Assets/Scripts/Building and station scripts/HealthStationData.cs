using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthStationData : ShipStationData
{
    public float healAmount = 20;
    // Start is called before the first frame update
    public override void onInteract(GameObject interactableObject)
    {
        PlayerHealthScript.CurrentPlayerHealthScript.Heal(healAmount);
        HudUiManager.HudManager.ShowHealthChange(healAmount*GlobalSceneManager.Healing, true);
    }
    
    public override void UpdateDisplayString(int currentUses, float currentCooldown)
    {
        string interactKey = "E";
        displayString = string.Format("Press {0} to heal {1} health\nUses Left: {2}\nCooldown: {3}", interactKey, 
            healAmount, currentUses, Mathf.Max(useCooldown-currentCooldown, 0).ToString("0.00"));
    }
}
