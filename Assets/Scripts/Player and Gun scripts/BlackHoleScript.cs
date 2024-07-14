using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class BlackHoleScript : ProjectileScript
{
    public float existingDuration = 4;
    public float damage = 50;
    public LayerMask StoppingLayers;
    private Rigidbody thisRigid;
    public float attractionRange;
    public float attractionStrength;
    public float TimeStart = 0f;
    public void Start()
    {
        thisRigid = this.GetComponent<Rigidbody>();
        Destroy(this.gameObject,existingDuration);
        TimeStart = Time.time;
    }

    public void FixedUpdate()
    {
        Collider[] hitColliders;
        String[] enemyLayers = { "Enemy" };
        Vector3 boxPosition = transform.position;
        hitColliders = Physics.OverlapSphere(boxPosition, attractionRange, LayerMask.GetMask(enemyLayers));
        List<PlayerCanHit> hitEnemies = hitColliders.ToList().Select(X => X.transform.root.GetComponent<PlayerCanHit>()).ToList();
        foreach (PlayerCanHit hitEnemy in hitEnemies)
        {
            Vector3 differenceVector = hitEnemy.transform.position - transform.position;
            float distance = differenceVector.magnitude;
            if (distance != 0)
            {
                hitEnemy.knockback(differenceVector * -attractionStrength,Time.fixedDeltaTime);
            }
            hitEnemy.damage(damage * Time.fixedDeltaTime,PlayerGunScript.currentGunScript.gameObject);
            
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        if ((StoppingLayers & 1 << other.gameObject.layer) != 0)
        {
            // if in layer.
            thisRigid.velocity = Vector3.zero;
        }
    }
}
