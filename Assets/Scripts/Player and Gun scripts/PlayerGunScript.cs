using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerGunScript : MonoBehaviour
{
    public static PlayerGunScript currentGunScript;
    // Start is called before the first frame update
    public Gun[] equippedGuns;
    public string lastUsedGunName;
    public int currentEquipped = 0;
    public float gunCooldown = 0;
    public float reloadTimer = 0;
    public bool reloading = false;
    public ProjectileScript currentGrenadeEquipped;
    public float grenadeThrowingStrength;
    public Vector3 handForGrenadePosition = Vector3.zero;
    public int grenadeCount;
    public float weaponSwitchTimer;
    public float currentTotalWeaponSwitchTime;
    public Camera gunCamera;
    public Gun currentGun
    {
        get { return equippedGuns[currentEquipped]; }   // get method
        set { Debug.Log("STOP TRYING TO SET GUN USING SETTER");}  // set method
    }

    public void Awake()
    {
        currentGunScript = this;
        equippedGuns = new Gun[gameManagerScript.manager.gunDatas.Length];
        for (int i = 0; i < equippedGuns.Length; i++)
        {
            equippedGuns[i] = new Gun(i);
        }
    }
    public void Start()
    {

        for (int i = 0; i < equippedGuns.Length;i++) {
            GameObject newGun = Instantiate(equippedGuns[i].data.gunObject, gunCamera.transform);
            // newGun.layer = LayerMask.NameToLayer("Gun"); // Use this to avoid weird collisions
            GlobalSceneManager.RecursiveLayerAssign(newGun, "Gun");
            equippedGuns[i].physicalGun = newGun;
            equippedGuns[i].physicalGunAnimationController = newGun.GetComponent<gunAnimationController>();
            equippedGuns[i].physicalGunAudioSource= newGun.GetComponent<AudioSource>();
            equippedGuns[i].physicalGunParticleSystem = newGun.GetComponentInChildren<ParticleSystem>();
            newGun.SetActive(currentEquipped == i);
        }

        lastUsedGunName = currentGun.data.gunName;
    }

    public void FixedUpdate()
    {
        if (gunCooldown > 0f)
        {
            gunCooldown -= Time.fixedDeltaTime;
        }

        
    }

    public void Update()
    {
        currentGun.update(PlayerControlScript.currentPlayer.playerCamera, this);
        
        //weapon switching fade/size.
        if (weaponSwitchTimer > 0)
        {
            weaponSwitchTimer -= Time.deltaTime;
            
        }
        else
        {
            weaponSwitchTimer = 0;
        }

        float switchProgress = 0;
        if (currentTotalWeaponSwitchTime == 0)
        {
            switchProgress = 1;
        }
        else
        {
            switchProgress = 1-weaponSwitchTimer/currentTotalWeaponSwitchTime;
        }

        if (reloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer > currentGun.data.reloadSpeed)
            {
                reloading = false;
                currentGun.reload();
            }
        }
        if(!reloading)
        {
            reloadTimer = 0;
        }
        currentGun.physicalGun.transform.localScale = currentGun.data.gunObject.transform.localScale * switchProgress;
        
    }
    public void primaryFire(PlayerControlScript source)
    {
        if (gunCooldown > 0)
        {
            return;
        }
        if (currentGun.currentClip > 0)
        {
            //StartCoroutine(gunAnimCoroutine());
            setGunCooldownMax(1/currentGun.data.fireRate);
        }
        else
        {
            beginReload();
            return;
        }
        for (int i = 0; i < currentGun.data.shotCount; i++)
        {
            if (currentGun.currentClip > 0)
            {
                currentGun.fire(source.playerCamera, this);
                currentGun.currentClip--;
                reloading = false;
                reloadTimer = 0;
                // GlobalSceneManager.AddTotalShots(currentGun.data.gunName, currentGun.data.bulletPerShot);
                GlobalSceneManager.AddTotalShots(currentGun.data.gunName, 1);
                // GlobalSceneManager.AddDamageDealt(currentGun.data.name, currentGun.data.damage);
            }
            else
            {
                break;
            }
        }
    }
    public void altFire(PlayerControlScript source)
    {
        currentGun.altFire(source.playerCamera);
        // GlobalSceneManager.AddTotalShots(currentGun.data.gunName, 1);
    }
    public void selectWeapon(int weaponToEquip)
    {
        int newEquip = weaponToEquip % equippedGuns.Length;
        equip(newEquip);
    }
    public void cycleWeapon(int direction)
    {
        int newEquip = currentEquipped;
        newEquip += direction;
        newEquip += equippedGuns.Length;
        newEquip %= equippedGuns.Length;
        equip(newEquip);
    }

    private void equip(int newEquip)
    {
        if (newEquip != currentEquipped)
        {
            // delete all holograms.
            foreach (var VARIABLE in gameManagerScript.manager.buildingHolograms)
            {
                VARIABLE.SetActive(false);
            }
            

            reloading = false;
            // equip will work as follows
            // when equip button is pressed, current weapon immediately disapears, (but still isn't unequiped quite yet.
            // Then the selected weapon begins to appear, by rotating into frame. The rotation time is 
            // switch too time.
            // when weapon is fully pulled out, its 'equipped'.
            
            currentGun.physicalGun.SetActive(false);
            currentEquipped = newEquip;
            currentGun.physicalGun.SetActive(true);
            currentGun.physicalGun.transform.localScale = Vector3.zero;
            weaponSwitchTimer = currentGun.data.switchTooSpeed;
            currentTotalWeaponSwitchTime = currentGun.data.switchTooSpeed;
            setGunCooldownMax(weaponSwitchTimer);
        }

        lastUsedGunName = currentGun.data.gunName;
    }
    public bool beginReload()
    {
        if (currentGun.currentClip < currentGun.data.clipSize && currentGun.currentAmmo > 0 && gunCooldown <= 0)
        {
            if (!reloading)
            {
                currentGun.physicalGunAnimationController.call("Reload");
            }

            reloading = true;
            return true;
        }

        return false;
    }

    public void useGrenade()
    {
        if (grenadeCount <= 0)
        {
            return;
        }

        if (currentGrenadeEquipped == null)
        {
            Debug.LogWarning("NULL GRENADE EQUIPPED");
            return;
        }

        reloading = false;
        // code borrowed from Projectile Script.
        Camera playerCamera = PlayerControlScript.currentPlayer.playerCamera;
        Transform cameraTransform = playerCamera.transform;
        Vector3 shootDirection = cameraTransform.forward;
        shootDirection.Normalize();
        Vector3 shootFromPos = cameraTransform.position + cameraTransform.rotation * handForGrenadePosition;
        Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
        shootDirection = shootRotation * currentGrenadeEquipped.shootDirection.normalized;
        ProjectileScript newProjectile = Instantiate(currentGrenadeEquipped, shootFromPos, Quaternion.LookRotation(shootDirection));
        newProjectile.thisRigidBody.velocity = newProjectile.transform.forward * grenadeThrowingStrength;
        newProjectile.gunScriptManagerCreator = this;
        grenadeCount -= 1;
        GlobalSceneManager.AddTotalShots("Grenade", 1);
        GlobalSceneManager.AddGrenadesUsed();
        // Have to state grenade was last used
    }
    private void setGunCooldownMax(float newCooldown)
    {
        gunCooldown = Mathf.Max(newCooldown,gunCooldown);
    }
}
