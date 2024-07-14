using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileScript : MonoBehaviour
{
    public Rigidbody thisRigidBody;
    public GunData gunDataCreator; // can be null
    public Vector3 shootDirection = Vector3.forward;
    public PlayerGunScript gunScriptManagerCreator;

    // Start is called before the first frame update
    public void playParticleOnHit(GameObject explosionEffect)
    {
        GameObject explosionInstance = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        ParticleSystem particleTime = explosionInstance.GetComponent<ParticleSystem>();
        Destroy(explosionInstance, particleTime.main.duration + particleTime.main.startLifetime.constantMax);
        Destroy(this.gameObject);
    }

}
