using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int ammo = PlayerGunScript.currentGunScript.currentGun.currentAmmo;
        int clip = PlayerGunScript.currentGunScript.currentGun.currentClip;

        ammoText.text = clip + " | " + ammo;
    }
}
