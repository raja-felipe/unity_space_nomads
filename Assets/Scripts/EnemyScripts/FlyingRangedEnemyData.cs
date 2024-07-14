using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRangedEnemyData : EnemyData
{
    /// <summary>
    /// </summary>
    // Start is called before the first frame update
    public override Vector3 getNewTarget(EnemyControlScript enemyScript)
    {
        return PlayerControlScript.currentPlayer.transform.position;
    }

    public override GameObject getNewAttackTarget(EnemyControlScript enemyScript)
    {
        if (enemyScript.currentlyBlocked)
        {
            return enemyScript.currentBlocker;
        }
        return PlayerControlScript.currentPlayer.gameObject;
    }

    public override Vector3 getAttackPosition(EnemyControlScript enemyScript, GameObject target)
    {
        if (enemyScript.currentlyBlocked)
        {
            return enemyScript.currentBlocker.transform.position;
        }
        return PlayerControlScript.currentPlayer.transform.position;
    }

    public override void attack(GameObject target, EnemyControlScript thisEnemy)
    {
        
        ParticleSystem bulletParticle = Instantiate(attackParticle, thisEnemy.transform.position,
            thisEnemy.transform.rotation);
        bulletParticle.transform.localScale = new Vector3(bulletParticle.transform.localScale.x,
            bulletParticle.transform.localScale.y, (target.transform.position - thisEnemy.transform.position).magnitude);
        EnemyCanHit targetScript = target.GetComponent<EnemyCanHit>();
        targetScript.damage(damage, thisEnemy);
        
    }
}
