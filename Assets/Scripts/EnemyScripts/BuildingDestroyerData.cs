using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingDestroyerScript : EnemyData
{
    /// <summary>
    /// This script describes the buildingDestroyer. Who should prioritise destroying buildings. And the nearby objectives.
    /// </summary>
    public Collider currentTargetObjective;

    public float buildingDestroyingRange = 10f;
    public float buildingDamageMultiplier = 3f;

    public GameObject currentTarget;
    
    public override void onSpawn(EnemyControlScript enemyScript)
    {
        
        currentTargetObjective = gameManagerScript.manager.random_objective().GetComponent<Collider>();
        //currentTargetObjective = gameManagerScript.manager.closest_objective(enemyScript.transform.position).GetComponent<Collider>();
    }
    
    public override Vector3 getNewTarget(EnemyControlScript enemyScript)
    {
        if (enemyScript.currentlyBlocked && enemyScript.currentBlocker != null)
        {
            currentTarget = enemyScript.currentBlocker;
            return currentTarget.transform.position;
        }
        Collider[] nearbyBuildingsColliders = Physics.OverlapSphere(enemyScript.transform.position, buildingDestroyingRange, navMeshManager.meshManager.ShortBlockingLayers);
        List<buildableObjectScript> nearbyBuildings =
            nearbyBuildingsColliders.Select(x => x.transform.root.GetComponent<buildableObjectScript>()).ToList();
        if (nearbyBuildings.Count <= 0)
        {
            currentTarget = currentTargetObjective.gameObject;
        }
        else
        {
            float m = Mathf.Infinity;
            buildableObjectScript ret = null;
            foreach(buildableObjectScript x in nearbyBuildings)
            {
                float dist = (x.transform.position - enemyScript.transform.position).magnitude;
                if (dist < m)
                {
                    m = dist;
                    ret = x;
                }
            }
            currentTarget = ret.gameObject;
        }
        // target whichever objective is closest from spawn. This cuts down runtime.
        
        return currentTarget.transform.position;
    }
    public override GameObject getNewAttackTarget(EnemyControlScript enemyScript)
    {
        
        return currentTarget;
    }

    public override Vector3 getAttackPosition(EnemyControlScript enemyScript,GameObject target)
    {
        return target.transform.position;
    }

    public override void attack(GameObject target,EnemyControlScript thisEnemy)
    {
        EnemyCanHit targetScript = target.GetComponent<EnemyCanHit>();
        if (targetScript.GetType() == typeof(buildableObjectScript))
        {
            
            targetScript.damage(damage * buildingDamageMultiplier, thisEnemy);
        }
        else
        {
            targetScript.damage(damage, thisEnemy);
        }
    }
}
