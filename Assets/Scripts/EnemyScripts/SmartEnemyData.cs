using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartEnemyData: EnemyData
{

    public new string name = "Smarter Bob";
    public GameObject currentTarget;
    public float playerRangeMultiplier;
    public float playerRangeAdd;
    public override Vector3 getNewTarget(EnemyControlScript enemyScript)
    {
        Collider currentTargetCollider = gameManagerScript.manager.closest_objective(this.transform.position).GetComponent<Collider>();
        Vector3 closestObjPosition = currentTargetCollider.transform.position;
        Vector3 playerPos = PlayerControlScript.currentPlayer.transform.position;
        if (playerRangeMultiplier * (transform.position - closestObjPosition).magnitude + playerRangeAdd<  (transform.position - playerPos).magnitude)
        {
            currentTarget = currentTargetCollider.gameObject;
            return currentTargetCollider.ClosestPoint(this.transform.position);
        }
        else
        {
            currentTarget = PlayerControlScript.currentPlayer.gameObject;
            return currentTarget.transform.position;
        }
    }
    public override GameObject getNewAttackTarget(EnemyControlScript enemyScript)
    {
        if (enemyScript.currentlyBlocked)
        {
            return enemyScript.currentBlocker;
        }
        return currentTarget;
    }
    public override Vector3 getAttackPosition(EnemyControlScript enemyScript, GameObject target)
    {
        if (enemyScript.currentlyBlocked)
        {
            return enemyScript.currentBlocker.transform.position;
        }
        return currentTarget.transform.position;
    }
    public override void attack(GameObject target,EnemyControlScript thisEnemy)
    {
        EnemyCanHit targetScript = target.GetComponent<EnemyCanHit>();
        targetScript.damage(damage, thisEnemy);
    }
}
