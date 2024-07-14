using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GrenadeBulletScript : ProjectileScript
{
    public float AOE = 1;
    public float detonationTime = 4;
    public float damage = 50;
    public LayerMask detonateLayers = Physics.AllLayers;
    public LayerMask enemyLayers = Physics.AllLayers;
    public GameObject explosionEffect;
    public void Start()
    {
        Debug.Log(transform.forward);
        StartCoroutine(explodeCoroutine());
    }

    public IEnumerator explodeCoroutine()
    {
        yield return new WaitForSeconds(detonationTime);
        explode();
        yield return null;
    }
    public void OnCollisionEnter(Collision other)
    {
        //if hit layer is in layermask.
        if(detonateLayers == (detonateLayers | (1 <<other.gameObject.layer)))
        {
            explode();
        }
    }

    public void explode()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(this.transform.position, AOE, enemyLayers);
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            PlayerCanHit hitEnemy = hitEnemies[i].transform.root.GetComponent<PlayerCanHit>();
            hitEnemy.damage(damage,transform.gameObject);
            // Knockback here.
        }
        playParticleOnHit(explosionEffect);
        
    }
}
