using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemyData : EnemyData
{
    /// <summary>
    /// This script is describes the common enemy. It should be attached to the prefab.
    /// </summary>
    /// 
    public Collider currentTargetCollider;


    public Vector3 targetPosition;
    

    public override void onSpawn(EnemyControlScript enemyScript)
    {
        currentTargetCollider = gameManagerScript.manager.random_objective().GetComponent<Collider>();
        //currentTargetCollider = gameManagerScript.manager.closest_objective(enemyScript.transform.position).GetComponent<Collider>();
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
        if (enemyScript.currentlyBlocked)
        {
            return enemyScript.currentBlocker;
        }
        return currentTargetCollider.gameObject;
    }

    public override Vector3 getAttackPosition(EnemyControlScript enemyScript,GameObject target)
    {
        
        //nearest objective or blocker
        if (enemyScript.currentlyBlocked)
        {
            return target.transform.position;
        }
        return targetPosition;
    }

    public override void attack(GameObject target,EnemyControlScript thisEnemy)
    {
        EnemyCanHit targetScript = target.GetComponent<EnemyCanHit>();
        targetScript.damage(damage, thisEnemy);
    }
}
