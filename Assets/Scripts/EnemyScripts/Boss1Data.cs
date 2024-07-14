using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Data : EnemyData
{
    /// <summary>
    /// This script is describes the common enemy. It should be attached to the prefab.
    /// </summary>
    public Collider currentTargetCollider;
    

    public Vector3 targetPosition;
    public ParticleSystem smallAttackParticle;
    public float smallGunRange = 5f;
    public float smallGunDamage;
    public float smallGunFirerate;

    public override void onSpawn(EnemyControlScript enemyScript)
    {
        float lowestHealth = Mathf.Infinity;
        ObjectiveScript lowestHealthObjective = null;
        foreach (var VARIABLE in 
                 gameManagerScript.manager.objectives)
        {
            if (VARIABLE.currentHealth < lowestHealth)
            {
                lowestHealth = VARIABLE.currentHealth;
                lowestHealthObjective = VARIABLE;
            }
        }

        currentTargetCollider = lowestHealthObjective.GetComponent<Collider>();

    }

    public override Vector3 getNewTarget(EnemyControlScript enemyScript)
    {
        // target whichever objective is closest from spawn. This cuts down runtime.
        targetPosition = currentTargetCollider.ClosestPoint(enemyScript.transform.position);
        //nearest objective 
        return targetPosition;
    }
    public override GameObject getNewAttackTarget(EnemyControlScript enemyScript)
    {
        //nearest objective or blocker
        // check if player is visible and in range of small guns
        // if so, attack player
        // otherwise set it to objective.
        if (enemyScript.currentlyBlocked)
        {
            return enemyScript.currentBlocker;
        }

        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(enemyScript.transform.position,
               (PlayerHealthScript.CurrentPlayerHealthScript.transform.position-enemyScript.transform.position).normalized,
               out hit,
               smallGunRange, 
               ~LayerMask.GetMask("Enemy")))
        {
            if ((LayerMask.GetMask("Player") & (1 << hit.collider.gameObject.layer)) != 0)
            {
                return PlayerHealthScript.CurrentPlayerHealthScript.gameObject;
            }
        }
        return currentTargetCollider.gameObject;
    }

    public override Vector3 getAttackPosition(EnemyControlScript enemyScript,GameObject target)
    {
        ObjectiveScript objScript = target.GetComponent<ObjectiveScript>();
        if (objScript != null)
        {
            return targetPosition;
        }
        
        return target.transform.position;
    }

    public override void attack(GameObject target,EnemyControlScript thisEnemy)
    {
        // if objective, attack with lasergun
        // if player, attack with mini guns.
        ObjectiveScript objScript = target.GetComponent<ObjectiveScript>();

        if (objScript != null)
        {
            if (attackParticle != null)
            {
                Vector3 thisPosition = attackFromPosition.transform.position;
                Vector3 targetPosition = target.transform.position;
                Quaternion targetDirection = thisEnemy.transform.rotation;
                ParticleSystem bulletParticle = Instantiate(attackParticle, thisPosition,targetDirection);
                bulletParticle.transform.localScale = new Vector3(bulletParticle.transform.localScale.x,
                    bulletParticle.transform.localScale.y, ( targetPosition - thisEnemy.transform.position).magnitude);
            }
            objScript.damage(damage, thisEnemy);
        }
        else
        {
            EnemyCanHit targetScript = target.GetComponent<EnemyCanHit>();
            
            ParticleSystem bulletParticle = Instantiate(smallAttackParticle, thisEnemy.transform.position,
                thisEnemy.transform.rotation);
            bulletParticle.transform.localScale = new Vector3(bulletParticle.transform.localScale.x,
                bulletParticle.transform.localScale.y, (target.transform.position - thisEnemy.transform.position).magnitude);
            targetScript.damage(smallGunDamage, thisEnemy);
            
            thisEnemy.attackCooldownTimer = smallGunFirerate;
        }
        
    }
    
    // Override onDeath function
    public override void onDeath(EnemyControlScript enemyScript)
    {
        base.onDeath(enemyScript);
        PlayerHealthScript.CurrentPlayerHealthScript.uiManager.GoToUpgrade();
    }
}
