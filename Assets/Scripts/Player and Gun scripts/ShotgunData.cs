using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class ShotgunData : GunData
{
    public ParticleSystem bulletParticle;
    public ParticleSystem bulletParticleOnHit;
    public override void fire(Camera playerCamera,PlayerGunScript gunScript)
    {
        hitscanFire(playerCamera,gunScript,bulletParticle,bulletParticleOnHit);
    }

    public override void altFire(Camera playerCamera)
    {
       //Debug.Log("SHOTGUN alt FIRE");
    }
}
