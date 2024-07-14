using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class AmmoStationData : ShipStationData
{
    public float ammoGainPercent = 0.2f;
    // Start is called before the first frame update
    public override void onInteract(GameObject interactableObject)
    {
        if (PlayerGunScript.currentGunScript.equippedGuns.Length == 0)
        {
            Debug.Log("NO GUNS??");
            return;
        }
        Gun primaryGun = PlayerGunScript.currentGunScript.equippedGuns[0];
        // set the current ammo to be the minimum of the increased am
        primaryGun.currentAmmo += Mathf.RoundToInt( primaryGun.data.totalAmmo *  ammoGainPercent );
        if (primaryGun.currentAmmo > primaryGun.data.totalAmmo)
        {
            primaryGun.currentAmmo = primaryGun.data.totalAmmo;
        }
    }
    
    public override void UpdateDisplayString(int currentUses, float currentCooldown)
    {
        string interactKey = "E";
        displayString = string.Format("Press {0} to refill {1}% of your current gun's ammo\nUses Left: {2}\nCooldown: {3}", 
            interactKey, ammoGainPercent*100, currentUses, Mathf.Max(useCooldown-currentCooldown, 0).ToString("0.00"));
    }
}
