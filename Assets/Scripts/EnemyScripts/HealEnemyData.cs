using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealEnemyData : EnemyData
{
    /// <summary>
    /// This script is describes the healing enemy.
    /// </summary>
    public GameObject currentTarget;
    public float visionRange = 10f;
    public LineRenderer line;

    public override Vector3 getNewTarget(EnemyControlScript enemyScript)
    {
        Collider[] nearbyEnemiesColliders = Physics.OverlapSphere(enemyScript.transform.position, visionRange, LayerMask.GetMask("Enemy"));
        List<EnemyControlScript> nearbyEnemies =
            nearbyEnemiesColliders.Select(x => x.transform.root.GetComponent<EnemyControlScript>()).ToList();
        if (nearbyEnemies.Count <= 0)
        {
            currentTarget = null;
        }
        else
        {
            float m = Mathf.Infinity;
            EnemyControlScript ret = null;
            foreach(EnemyControlScript x in nearbyEnemies)
            {
                float dist = x.currentHealth/x.data.maxHealth;
                if (dist < m)
                {
                    m = dist;
                    ret = x;
                }
            }
            currentTarget = ret.gameObject;
        }
        if (currentTarget == null)
        {
            return enemyScript.transform.position;
        }
        return currentTarget.transform.position;
    }
    public override GameObject getNewAttackTarget(EnemyControlScript enemyScript)
    {
        if (currentTarget == null)
        {
            return null;
        }
        if((currentTarget.transform.position - enemyScript.transform.position).magnitude < this.attackRange)
        {
            line.enabled = true;
            line.useWorldSpace = true;
            line.SetPosition(0, enemyScript.transform.position);
            line.SetPosition(1, currentTarget.transform.position);
        }
        else
        {
            line.useWorldSpace = false;
            line.positionCount = 2;
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
            line.enabled = false;
        }
        return currentTarget;
    }
    public override Vector3 getAttackPosition(EnemyControlScript enemyScript, GameObject target)
    {
        return target.transform.position;
    }
    public override void attack(GameObject target,EnemyControlScript thisEnemy)
    {
        EnemyControlScript targetFriend = target.GetComponent<EnemyControlScript>();
        if (targetFriend == null)
        {
            Debug.LogError(gameObject.name + " TRYING TO HEAL NON-ENEMY");
        }
        
        targetFriend.currentHealth = Mathf.Min(targetFriend.data.maxHealth, targetFriend.currentHealth - damage);
    }
}
