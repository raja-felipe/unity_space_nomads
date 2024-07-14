using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrenadeLauncherData : GunData
{
    public float AOE;
    public override void fire(Camera playerCamera,PlayerGunScript gunScript)
    {
        
        Transform cameraTransform = playerCamera.transform;
        for (int i = 0; i < bulletPerShot; i++)
        {
            //create a random point in a sphere.
            Vector3 randomSphere = Random.insideUnitSphere;
            // scale this point based on spread, collapses to an elipse.
            randomSphere.Scale(new Vector3(hspread, vspread, 0));
            // angle elipse so it is facing the player. An essential step, otherwise it faces the world axis.
            randomSphere = Quaternion.LookRotation(-playerCamera.transform.forward) * randomSphere;
            // place a bullet in front of the player, and then move it based on the random elipse position.
            Vector3 shootDirection = cameraTransform.forward + randomSphere;
            shootDirection.Normalize();
            Vector3 shootFromPos = cameraTransform.position + cameraTransform.rotation * muzzlePosition;
            // shoot direction is a position vector, which is in the direction the bullet will go.
            // target pos is the target position for the bullet.
            Debug.Log(shootDirection);
            Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
            shootDirection = shootRotation * bullet.shootDirection.normalized;
            
            ProjectileScript newProjectile = Instantiate(bullet, shootFromPos, Quaternion.LookRotation(shootDirection));
            if (bullet.GetType() == typeof(GrenadeBulletScript))
            {
                GrenadeBulletScript newGrenadeScript = (GrenadeBulletScript) newProjectile;
                newGrenadeScript.AOE = AOE;
            }

            newProjectile.thisRigidBody.velocity = newProjectile.transform.forward * bulletSpeed;
            newProjectile.gunDataCreator = this;
            newProjectile.gunScriptManagerCreator = gunScript;
        }
        
    }

    public override void altFire(Camera playerCamera)
    {
        //Debug.Log("GrenadeLauncherAltFIRE");
    }
}
