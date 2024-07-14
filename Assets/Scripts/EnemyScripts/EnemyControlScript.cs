using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControlScript : PlayerCanHit
{ 
    
    /// <summary>
    /// This script is attached to each of the enemies, and controls all the enemies in general.
    /// More unique functionality is in EnemyData children scripts.
    /// </summary>
    public float attackCooldownTimer = 0;
    public EnemyData data;
    public NavMeshAgent agent;
    public GameObject currentAttackTarget;
    public float speedMultiplier = 1;
    public GameObject currentBlocker;
    public bool currentlyBlocked;
    public float maxRotationSpeed;
    // Need these for displaying enemy health
    void Awake ()
    {
        data = GetComponent<EnemyData>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.speed = data.speed;
        agent.stoppingDistance = data.stopDistance;
        agent.baseOffset = data.heightAbove;
        agent.updateRotation = false;
        doOnAwake();
        maxHealth = data.maxHealth;
    }
    
    void OnEnable()
    {
        
        agent.enabled = true;
        // set short wall area costs.
        foreach (int areamask in navMeshManager.meshManager.allAreas)
        {
            float currentCost = agent.GetAreaCost(areamask);
            if (data.damage > 0 && data.attackSpeed > 0)
            {
                agent.SetAreaCost(areamask, currentCost * data.damage * data.attackSpeed + 1);
            }
            else
            {
                agent.areaMask = agent.areaMask & ~(1 << areamask);
            }
        }
        // set walk over area costs.
        foreach (int areamask in navMeshManager.meshManager.walkOverAreas)
        {
            float defaultCost = agent.GetAreaCost(navMeshManager.meshManager.defaultArea);
            agent.SetAreaCost(areamask,defaultCost);
        }
        // set fly over area costs.
        if (data.isFlying)
        {
            foreach (int areamask in navMeshManager.meshManager.flyOverAreas)
            {
                float defaultCost = agent.GetAreaCost(navMeshManager.meshManager.defaultArea);
                agent.SetAreaCost(areamask,defaultCost);
            }
        }
        
        speedMultiplier = 1;
        agent.speed = data.speed * speedMultiplier;
        currentBlocker = null;
        currentlyBlocked = false;
        currentHealth = data.maxHealth;
        data.onSpawn(this);
        StartCoroutine(targetUpdateRoutine());
        StartCoroutine(CheckForBlockersCoroutine());
        StartCoroutine(walkAnimRoutine());
    }
    public IEnumerator targetUpdateRoutine()
    {
        while (enabled)
        {
            if (agent.enabled)
            {
                agent.SetDestination(data.getNewTarget(this));
            }

            yield return new WaitForSeconds(data.newTargetUpdateDelay);
        }
        yield return null;
    }

    public float stepDuration = 0.1f;

    public IEnumerator walkAnimRoutine()
    {
        while (enabled)
        {
            if (agent.enabled)
            {
                if (Mathf.Floor(agent.velocity.magnitude) > 0.5f)
                {
                    data.call("Walk");
                }
            }

            yield return new WaitForSeconds(stepDuration);
        }

        yield return null;
    }
    void FixedUpdate()
    {
        currentAttackTarget = data.getNewAttackTarget(this);
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.fixedDeltaTime;
        }
        if(canAttack(currentAttackTarget)){
            Attack(currentAttackTarget);
            
            if (currentlyBlocked)
            {
                StartCoroutine(ReduceSpeedOnAttack(1));
            }
            else
            {
                StartCoroutine(ReduceSpeedOnAttack(data.attackSpeedReduction));
            }

        }
    }
    public bool fixVelocity = false;
    public Vector3 VelToFix = Vector3.zero;
    private void Update()
    {
        doOnUpdate();
        if (fixVelocity)
        {

            if (agent.enabled)
            {
                agent.velocity = VelToFix;
                agent.nextPosition += VelToFix * Time.deltaTime;
                fixVelocity = false;
            }
        }
        
        if (inRange && currentAttackTarget != null)
        {
            Vector3 targetRotationVector = currentAttackTarget.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(targetRotationVector), maxRotationSpeed* Time.deltaTime);
        }
        else
        {
            
            Vector3 targetRotationVector = agent.steeringTarget - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(targetRotationVector), maxRotationSpeed * Time.deltaTime);
        }

        if (this.isActiveAndEnabled)
        {
            if (data.thisAudioSource != null)
            {
                data.thisAudioSource.volume = GlobalSceneManager.EnemyVolume * this.data.defaultAudioVolume;
            }
        }
    }
    public bool inRange = true;
    public bool canAttack(GameObject target)
    {
        if (attackCooldownTimer > 0)
        {
            return false;
        }

        if (target == null)
        {
            return false;
        }
        
        Vector3 targetAttack = data.getAttackPosition(this,target);
        if (data.isRangedEnemy || !agent.enabled)
        {
            if (Vector3.Distance(targetAttack, transform.position) > data.attackRange)
            {
                
                if (inRange)
                {
                    inRange = false;
                    data.call("OutOfRange");
                }
                return false;
            }
            if (Physics.Linecast(transform.position, targetAttack, data.attackblockingLayers))
            {
                
                if (inRange)
                {
                    inRange = false;
                    data.call("OutOfRange");
                }
                return false;
            }
        }
        else
        {
            // this is more consistent, as melee attacks won't get blocked by small obstacles.
            if (!currentlyBlocked && agent.remainingDistance > data.attackRange)
            {
                if (inRange)
                {
                    inRange = false;
                    data.call("OutOfRange");
                }
                return false;
            }
        }
        if (!inRange)
        {
            inRange = true;
            data.call("InRange");
        }
        // if the distance from this gameobject, and the closest point on the target is in not range.
        return true;
    }
    public void Attack(GameObject target)
    {
        attackCooldownTimer = data.attackSpeed;
        data.attack(target,this);
        data.call("Attack");
    }

    public void onKill(EnemyCanHit killedObject)
    {
        data.onKill(this,killedObject);
    }
    
    public override float damage(float amount, GameObject source)
    {
        ProjectileScript damagingProjectile = source.GetComponent<ProjectileScript>();
        PlayerGunScript damagingPlayer = source.GetComponent<PlayerGunScript>();
        
        if (damagingProjectile != null)
        {
            // add score based on damagingProjectile.gundatacreator IF it is not null.
            if (damagingProjectile.gunDataCreator == null)
            {
                GlobalSceneManager.AddShotsHit("Grenade");
                GlobalSceneManager.AddDamageDealt("Grenade", amount);
            }

            else
            {
                GlobalSceneManager.AddShotsHit(damagingProjectile.gunDataCreator.gunName);
                GlobalSceneManager.AddDamageDealt(damagingProjectile.gunDataCreator.gunName, amount);
            }
        }
        
        else if (damagingPlayer != null)
        {
            // Now update the gun that hit the enemy
            // Debug.Log("ADDING SHOTS FOR: "+damagingPlayer.currentGun.data.gunName);
            GlobalSceneManager.AddShotsHit(damagingPlayer.currentGun.data.gunName);
            GlobalSceneManager.AddDamageDealt(damagingPlayer.currentGun.data.gunName, amount);   
        }

        if (amount <= 0)
        {
            return 0f;
        }
        
        currentHealth -= amount;
        data.whenHit(source,amount);
        float damageDealt = amount;
        if (currentHealth <= 0)
        {
            die();
        }
        // If they lived, display the health
        else
        {
            DisplayHealthBar();
        }
        
        return damageDealt;
    }

    public void die()
    {
        data.onDeath(this);
        if (data.resourcesOnKill > 0)
        {
            GooScript GooObject = gameManagerScript.manager.gooObject;
            GooScript newGooObject = Instantiate(GooObject, this.transform.position, this.transform.rotation);
            newGooObject.gooValue = data.resourcesOnKill;
        }

        if (data.explosionOnDeath != null)
        {
            ParticleSystem newExplosion = Instantiate(data.explosionOnDeath, transform.position, transform.rotation);
            newExplosion.transform.localScale = Vector3.one * data.explosionOnDeathSize;
            Destroy(newExplosion.gameObject, newExplosion.main.duration);
        }
        gameManagerScript.manager.returnToPool(this);
    }

    IEnumerator CheckForBlockersCoroutine()
    {
        yield return null;
        StartCoroutine(CheckBlockersOnCornerRoutine());
        while (true)
        {
            yield return new WaitForSeconds(data.checkBlockersDelay);
            (bool, GameObject) t = data.getAttackTargetObstructions(this);
            currentlyBlocked = t.Item1;
            currentBlocker = t.Item2;
        }
        yield return null;
        // CHECK FOR BLOCKERS REPEATEDLY
        // SET A BLOCKER VARIABLE
        // THIS VARIABLE CAN BE USED BY ENEMYDATA
    }

    IEnumerator CheckBlockersOnCornerRoutine()
    {
        yield return null;
        Vector3 nextCorner = this.transform.position;
        
        while (true)
        {
            yield return new WaitForEndOfFrame();
            
            if ((agent.path.corners.Length > 0) && nextCorner != agent.path.corners[0])
            {
                (bool, GameObject) t = data.getAttackTargetObstructions(this);
                nextCorner = agent.path.corners[0];
                currentlyBlocked = t.Item1;
                currentBlocker = t.Item2;
            }

        }
    }
    
    IEnumerator ReduceSpeedOnAttack(float amountToReduce)
    {
        speedMultiplier -= amountToReduce;
        if (agent.enabled)
        {
            agent.speed = data.speed * speedMultiplier;
        }

        yield return new WaitForSeconds(data.attackSpeed);
        speedMultiplier += amountToReduce;
        // Debug.Log("after " + speedMultiplier);
        if (agent.enabled)
        {
            agent.speed = data.speed * speedMultiplier;
        }
        yield return null;
    }

    public override void knockback(Vector3 direction,float duration)
    {
        if (agent != null)
        {
            StartCoroutine(KnockBackRoutine(direction, duration));
        }
    }

    IEnumerator KnockBackRoutine(Vector3 direction,float duration)
    {
        float start = Time.time;
        while (Time.time < start + duration)
        {
            agent.Move(new Vector3(direction.x,0,direction.z) * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        /*
        agent.enabled = false;
        thisRigid.isKinematic = false;
        thisRigid.AddForce(direction);
        yield return new WaitForSeconds(duration);
        // maybe fix the position of nearest navmesh and fix to it.
        NavMeshHit hit = new NavMeshHit();
        
        if (NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {

            transform.position = hit.position + new Vector3(0, data.heightAbove * transform.localScale.y, 0);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " COULDNT WARP :(");
        }
        agent.enabled = true;
        agent.Warp(transform.position);
        thisRigid.isKinematic = true;
        agent.speed = data.speed * speedMultiplier;

        yield return new WaitForEndOfFrame();
        agent.SetDestination(data.getNewTarget(this));
        */
    }
    
}
