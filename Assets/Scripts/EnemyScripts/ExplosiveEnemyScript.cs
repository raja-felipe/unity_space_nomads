using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemyScript : EnemyData
{
    public float AOE;
    public LayerMask LayersToHit;
    public float explosionTime;
    public bool exploding = false;
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
        if (!exploding)
        {
            StartCoroutine(ExplodeIn(thisEnemy, explosionTime));
            
        }
    }

    public IEnumerator ExplodeIn(EnemyControlScript thisEnemy, float timer)
    {
        yield return new WaitForSeconds(timer);
        //thisEnemy.agent.speed = 0;
        explode(thisEnemy);
        yield return null;
    }
    public void explode(EnemyControlScript thisEnemy)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(this.transform.position, AOE, LayersToHit);
        //damages buildings, objectives, player, and enemies.
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            PlayerCanHit hitEnemy = hitEnemies[i].transform.root.GetComponent<PlayerCanHit>();
            EnemyCanHit hitHittable = hitEnemies[i].transform.root.GetComponent<EnemyCanHit>();
            if (hitEnemy != null)
            {
                hitEnemy.damage(damage, this.gameObject);
                
            }
            if (hitHittable != null)
            {
                hitHittable.damage(damage,thisEnemy);
            } 
        }
        ParticleSystem explosionInstance = Instantiate(explosionOnDeath, this.transform.position, Quaternion.identity);
        Destroy(explosionInstance, explosionInstance.main.duration + explosionInstance.main.startLifetime.constantMax);
        
        thisEnemy.die();
    }
}
