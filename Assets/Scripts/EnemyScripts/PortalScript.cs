using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : PlayerCanHit
{
    public EnemyData enemyType;
    public float delayBetweenSpawns;
    public ParticleSystem onDeathParticle;
    public void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(spawnRoutine());
        doOnAwake();
    }

    private void Update()
    {
        doOnUpdate();
    }

    public IEnumerator spawnRoutine()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(delayBetweenSpawns);
            gameManagerScript.manager.createEnemy(this.transform.position, this.transform.rotation, enemyType);
        }
        yield return null;
    }
    public override void knockback(Vector3 direction, float duration)
    {
        return;
    }
    public override float damage(float amount, GameObject source)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            die();
        }
        // If they lived, display the health
        else
        {
            DisplayHealthBar();
        }
        return 0;
    }

    public void die()
    {
        if (onDeathParticle != null)
        {
            Instantiate(onDeathParticle, this.transform.position, this.transform.rotation);
        }
        Destroy(this.gameObject);
    }

}
