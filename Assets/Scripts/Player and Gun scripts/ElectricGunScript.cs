using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu]
public class ElectricGunScript : GunData
{
    public ParticleSystem electricityParticle;
    public ParticleSystem electricityExplosion;
    public float chainRange = 1f;
    public int chainCount = 5;
    public override void fire(Camera playerCamera,PlayerGunScript gunScript)
    {
        Transform cameraTransform = playerCamera.transform;
        
        for (int i = 0; i < bulletPerShot; i++)
        {
            Vector3 shootDirection = cameraTransform.forward;
            shootDirection.Normalize();
            Vector3 shootFromPos = cameraTransform.position;
            Vector3 particleFromPos = shootFromPos + cameraTransform.rotation * muzzlePosition;
            Vector3 particleDirection = shootDirection;
            
            RaycastHit hit;
            bool didHit = false;
            if (Physics.Raycast(shootFromPos, shootDirection, out hit, Mathf.Infinity,~gameManagerScript.manager.ShootThroughLayers))
            {
                Debug.Log(hit.collider.transform.root.name + " " + hit.collider.gameObject.name + " " + hit.collider.gameObject.layer);
                PlayerCanHit enemyScript = hit.collider.transform.root.GetComponent<PlayerCanHit>();
                damageEnemyWithElectricty(gunScript,enemyScript,hit.point);
                particleDirection = (hit.point -particleFromPos).normalized;
                didHit = true;
                
                chainToNearbyEnemies(hit.point,chainCount,new HashSet<PlayerCanHit>(){enemyScript}, gunScript);
            }
            if (electricityParticle != null)
            {
                ParticleSystem bulletParticle = Instantiate(electricityParticle, particleFromPos,
                    Quaternion.LookRotation(particleDirection));
                if (didHit)
                {
                    bulletParticle.transform.localScale = new Vector3(bulletParticle.transform.localScale.x,
                    bulletParticle.transform.localScale.y, (hit.point - particleFromPos).magnitude);
                }
                bulletParticle.Play();
                Destroy(bulletParticle.gameObject,bulletParticle.main.duration);
            }
        }
    }

    private void damageEnemyWithElectricty(PlayerGunScript gunScript, PlayerCanHit enemyScript,Vector3 hitPosition )
    {
        if (enemyScript != null)
        {
            enemyScript.damage(damage, gunScript.gameObject);
            if (electricityExplosion != null)
            {
                ParticleSystem onHitParticle = Instantiate(electricityExplosion, hitPosition,
                    Quaternion.identity);
                onHitParticle.Play();
                Destroy(onHitParticle.gameObject, onHitParticle.main.duration);
            }
        }
    }
    private int chainToNearbyEnemies(Vector3 currentPosition, int remainingChains,
        HashSet<PlayerCanHit> enemiesAlreadyHit,PlayerGunScript gunScript)
    {
        // find closest enemies, hit them
        // for each enemy hit, chain from them. Use BFS.
        String[] enemyLayers = { "Enemy" };
        Collider[] NearbyEnemyColliders = Physics.OverlapSphere(currentPosition, chainRange, LayerMask.GetMask(enemyLayers));

        List<PlayerCanHit> nearbyEnemyList =
            Enumerable.ToHashSet(NearbyEnemyColliders.ToList()
                    .Select(x => x.transform.root.GetComponent<PlayerCanHit>())
                )
                .Where(x => !enemiesAlreadyHit.Contains(x))
                .ToList();
        foreach (PlayerCanHit nearbyEnemy in nearbyEnemyList)
        {
            // chain damage from the current enemy to a new nearby enemy.
            if (remainingChains <= 0)
            {
                return 0;
            }
            Vector3 targetPosition = nearbyEnemy.transform.position;
            damageEnemyWithElectricty(gunScript,nearbyEnemy,targetPosition); // damage and burst particles
            // hit enemy
            // particles
            // directed particle
            if (electricityParticle != null)
            {
                ParticleSystem bulletParticle = Instantiate(electricityParticle, currentPosition,
                    Quaternion.LookRotation(targetPosition - currentPosition));
                bulletParticle.transform.localScale = new Vector3(bulletParticle.transform.localScale.x, 
                    bulletParticle.transform.localScale.y, (targetPosition - currentPosition).magnitude);
                
                bulletParticle.Play();
                Destroy(bulletParticle.gameObject,bulletParticle.main.duration);
            }
            enemiesAlreadyHit.Add(nearbyEnemy);
            remainingChains--;
            // reduce n
        }
        // start chaining for each of the enemies hit.
        // Doesn't actually do BFS. it does do all children before starting any subchild.
        foreach (PlayerCanHit nearbyEnemy in nearbyEnemyList)
        {
            remainingChains = chainToNearbyEnemies(nearbyEnemy.transform.position,remainingChains,enemiesAlreadyHit,gunScript);
            if (remainingChains <= 0)
            {
                return 0;
            }
        }

        return remainingChains;
    }

    public override void altFire(Camera playerCamera)
    {
        //Debug.Log("SHOTGUN alt FIRE");
    }
}
