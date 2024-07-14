using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    /// <summary>
    /// each gun contains gun data, which describes what gun it is. This is done so that multiple types of guns can
    /// exists in the future, ie picking up new guns, and also gun data can be easily written in the editor.
    /// 
    /// </summary>
    public GunData data;
    public int currentClip = 0;
    public int currentAmmo = 0;
    public GameObject physicalGun;
    public gunAnimationController physicalGunAnimationController;
    public AudioSource physicalGunAudioSource;
    public ParticleSystem physicalGunParticleSystem;

    public Gun(int type = 0)
    {
        if (type < gameManagerScript.Guns.Length)
        {
            data = gameManagerScript.Guns[type];
        }
        else
        {
            Debug.Log("GUN TYPE NOT FOUND");
        }

        currentAmmo = data.totalAmmo;
        currentClip = data.startingClip;
    }
    public void fire(Camera playerCamera,PlayerGunScript gunScript)
    {
        data.fire(playerCamera,gunScript);
        if (physicalGunAnimationController != null)
        {
            physicalGunAnimationController.call("Shoot");
        }
    }
    public void altFire(Camera playerCamera)
    {
        data.altFire(playerCamera);
    }

    public void update(Camera playerCamera, PlayerGunScript gunScript)
    {
        data.update(playerCamera,gunScript);
    }
    public void reload()
    {
        data.onReload(PlayerGunScript.currentGunScript);
        // sees how much can be reloaded, taking into account how many bullets reload at once, how much ammo we have, and how much we need.
        int AmountToReload = Math.Min(Math.Min(data.reloadPerReload, data.clipSize - currentClip), currentAmmo);
        if (AmountToReload > 0)
        {
            // animation.
        }
        currentAmmo -= AmountToReload;
        currentClip += AmountToReload;
    }
}
