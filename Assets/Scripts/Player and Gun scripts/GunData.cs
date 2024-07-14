using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GunData : ScriptableObject
{
    public string gunName;
    public float damage;
    public float fireRate;
    public int clipSize;
    public int startingClip;
    public int totalAmmo;
    public float reloadSpeed;
    public int shotCount = 1; // how many times the gun is shot per button press.
    public int bulletPerShot = 1; // how many bullets per shot;
    public float hspread = 0;
    public float vspread = 0;
    public int reloadPerReload = 1; //how much clip is reloaded per reload.
    public float switchTooSpeed = 0.5f;
    public ProjectileScript bullet = null; //instatiated when shot
    public Vector3 muzzlePosition = Vector3.zero;
    public GameObject gunObject = null; //does stuff when shot, e.g recoil.
    public float shootDuration = 0.1f;
    public float bulletSpeed;
    // Adding this to store icons for UI
    public Sprite gunSprite;
    public string gunDescription;

    public GunUpgrade[] possibleUpgrades;
    // RAJA: I don't know about the cost but tracking the currentUpgrades
    // is so much easier if we have a dictionary instead
    public Dictionary<GunUpgrade, bool> currentUpgrades = new Dictionary<GunUpgrade, bool>();
    
    public virtual void fire(Camera playerCamera,PlayerGunScript gunScript)
    {
        return;
    }

    public virtual void update(Camera playerCamera, PlayerGunScript gunScript)
    {
        return;
    }

    public abstract void altFire(Camera playerCamera);

    public void hitscanFire(Camera playerCamera, PlayerGunScript gunScript)
    {
        hitscanFire(playerCamera,gunScript,null,null);
    }
    public void hitscanFire(Camera playerCamera,PlayerGunScript gunScript, ParticleSystem bulletParticleEffect, ParticleSystem bulletOnHitParticle )
    {
        Transform cameraTransform = playerCamera.transform;
        
        for (int i = 0; i < bulletPerShot; i++)
        {
            //create a random point in a sphere.
            Vector3 randomSphere = Random.insideUnitSphere;
            // scale this point based on spread, collapses to an elipse.
            randomSphere.Scale(new Vector3(hspread, vspread, 0));
            // angle elipse so it is facing the player. An essential step, otherwise it faces the world axis.
            randomSphere = Quaternion.LookRotation(-playerCamera.transform.forward) * randomSphere;
            // place a bullet in front of the player, and then move it based on the random elipse position.
            Vector3 shootDirection = cameraTransform.forward + randomSphere;
            shootDirection.Normalize();
            Vector3 shootFromPos = cameraTransform.position; // + cameraTransform.rotation * muzzlePosition;
            Vector3 particleFromPos = shootFromPos + cameraTransform.rotation * muzzlePosition;
            Vector3 particleDirection = shootDirection;
            // shoot direction is a position vector, which is in the direction the bullet will go.
            // target pos is the target position for the bullet.
            RaycastHit hit;
            bool didHit = false;
            if (Physics.Raycast(shootFromPos, shootDirection, out hit, Mathf.Infinity,~gameManagerScript.manager.ShootThroughLayers))
            {
                PlayerCanHit enemyScript = hit.collider.transform.root.GetComponent<PlayerCanHit>();
                if (enemyScript != null)
                {
                    float total_damage = enemyScript.damage(damage, gunScript.gameObject);
                    onHit(gunScript, enemyScript, total_damage);
                }
                particleDirection = (hit.point -particleFromPos).normalized;
                didHit = true;
            }
            if (bulletParticleEffect != null)
            {
                
                ParticleSystem bulletParticle = Instantiate(bulletParticleEffect, particleFromPos,
                    Quaternion.LookRotation(particleDirection));
                
                if (didHit)
                {
                    bulletParticle.transform.localScale = new Vector3(bulletParticle.transform.localScale.x,
                    bulletParticle.transform.localScale.y, (hit.point - particleFromPos).magnitude);
                    if (bulletOnHitParticle != null)
                    {
                        ParticleSystem onHitParticle = Instantiate(bulletOnHitParticle, hit.point,
                            Quaternion.LookRotation(particleDirection));
                        Destroy(onHitParticle.gameObject, onHitParticle.main.duration);
                    }
                }
                Destroy(bulletParticle.gameObject,bulletParticle.main.duration);
            }
        }
    }
    public virtual void onHit(PlayerGunScript source, PlayerCanHit target, float total_damage)
    {
        return;
    }
    public virtual void onReload(PlayerGunScript source)
    {
        return;
    }
    
    // Override equality function for GunData
    public override bool Equals(object other)
    {
        GunData item = (GunData)other;

        if (item == null) return false;
        
        return this.gunName.Equals(item.gunName);
    }

    public override int GetHashCode()
    {
        return this.gunName.GetHashCode();
    }
    
    // Need this function to check for possession of a given GunUpgrade
    public bool HasGunUpgrade(GunUpgrade upgrade)
    {
        return currentUpgrades.ContainsKey(upgrade);
    }
    
    public bool HasGunUpgrade(string gunUpgradeName)
    {
        GunUpgrade referredToUpgrade = FindGunUpgradeByName(gunUpgradeName);
        return currentUpgrades.ContainsKey(referredToUpgrade);
    }
    
    // Several helper functions to apply the gun upgrades from teh GunData attribute
    public void ApplyUpgrade(GunUpgrade upgrade)
    {
        if (!currentUpgrades.ContainsKey(upgrade))
        {
            upgrade.apply(this);
            currentUpgrades.TryAdd(upgrade, true);
        }
    }
    
    
    public void UnApplyUpgrade(GunUpgrade upgrade)
    {
        if (currentUpgrades.ContainsKey(upgrade))
        {
            upgrade.unApply(this);
            currentUpgrades.Remove(upgrade);
        }
    }
    
    public void ApplyUpgrade(int index)
    {
        if (index >= possibleUpgrades.Length) return;
        
        GunUpgrade currUpgrade = possibleUpgrades[index];
        if (!currentUpgrades.ContainsKey(currUpgrade))
        {
            currUpgrade.apply(this);
            currentUpgrades.TryAdd(currUpgrade, true);
        }
    }
    
    public void UnApplyUpgrade(int index)
    {
        if (index >= possibleUpgrades.Length) return;
        
        GunUpgrade currUpgrade = possibleUpgrades[index];
        if (currentUpgrades.ContainsKey(currUpgrade))
        {
            currUpgrade.unApply(this);
            currentUpgrades.Remove(currUpgrade);
        }
    }

    public void ApplyUpgrade(string gunUpgradeName)
    {
        GunUpgrade currUpgrade = FindGunUpgradeByName(gunUpgradeName);
        if (!currentUpgrades.ContainsKey(currUpgrade))
        {
            currUpgrade.apply(this);
            currentUpgrades.TryAdd(currUpgrade, true);
        }
    }
    
    public void UnApplyUpgrade(string gunUpgradeName)
    {
        GunUpgrade currUpgrade = FindGunUpgradeByName(gunUpgradeName);
        if (currentUpgrades.ContainsKey(currUpgrade))
        {
            currUpgrade.unApply(this);
            currentUpgrades.Remove(currUpgrade);
        }
    }

    private GunUpgrade FindGunUpgradeByName(string gunUpgradeName)
    {
        for (int i = 0; i < possibleUpgrades.Length; i++)
        {
            GunUpgrade currUpgrade = possibleUpgrades[i];
            if (currUpgrade.name == gunUpgradeName) return currUpgrade;
        }
        return null;
    }
}
