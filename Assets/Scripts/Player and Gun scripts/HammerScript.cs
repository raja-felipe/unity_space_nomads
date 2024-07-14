using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class HammerScript : GunData
{
    // attacks enemies with a big square shaped swing.
    public Vector3 hitDimensions;
    public float forwardOffset;
    public float knockBackAmount;
    public float knockBackDuration;
    public float repairRange = 1f;
    public float repairAmount;
    public float repairEfficiency = 2f; // how much health is repaired per material. NOT 0.
    public float repairDelay = 0.5f;
    public bool canRepair;
    public void DrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c, float duration)
    {
        // create matrix
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, scale);
 
        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));
 
        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));
 
        Debug.DrawLine(point1, point2, c,duration);
        Debug.DrawLine(point2, point3, c,duration);
        Debug.DrawLine(point3, point4, c,duration);
        Debug.DrawLine(point4, point1, c,duration);
 
        Debug.DrawLine(point5, point6, c,duration);
        Debug.DrawLine(point6, point7, c,duration);
        Debug.DrawLine(point7, point8, c,duration);
        Debug.DrawLine(point8, point5, c,duration);
 
        Debug.DrawLine(point1, point5, c,duration);
        Debug.DrawLine(point2, point6, c,duration);
        Debug.DrawLine(point3, point7, c,duration);
        Debug.DrawLine(point4, point8, c,duration);
 
    }
    public override void fire(Camera playerCamera, PlayerGunScript gunScript)
    {
        // do a collider rectangle check some distance in front of player.
        Collider[] hitColliders;
            
        String[] enemyLayers = { "Enemy" };
        Vector3 boxPosition = playerCamera.transform.position + playerCamera.transform.forward * forwardOffset;
        Quaternion boxRotation = playerCamera.transform.rotation;
        hitColliders = Physics.OverlapBox(boxPosition, hitDimensions, boxRotation, LayerMask.GetMask(enemyLayers));
        DrawBox(boxPosition,  boxRotation,hitDimensions * 2, Color.green,1);
        List<PlayerCanHit> hitEnemies = hitColliders.ToList().Select(X => X.transform.root.GetComponent<PlayerCanHit>()).ToList();
        foreach (PlayerCanHit hitEnemy in hitEnemies)
        {
            Vector3 differenceVector = hitEnemy.transform.position - gunScript.transform.forward;
            hitEnemy.knockback(playerCamera.transform.forward * knockBackAmount,knockBackDuration);
            hitEnemy.damage(damage,gunScript.gameObject);
        }
        gunScript.currentGun.currentClip += 1;
    }
    /// <summary>
    /// Repairs buildings on alt fire.
    /// </summary>
    public override void altFire(Camera playerCamera)
    {
        if (canRepair == false)
        {
            return;
        }
        Transform cameraTransform = playerCamera.transform;
        Vector3 shootDirection = cameraTransform.forward;
        shootDirection.Normalize();
        Vector3 shootFromPos = cameraTransform.position + cameraTransform.rotation * muzzlePosition;
        RaycastHit hit;
        if (Physics.Raycast(shootFromPos, shootDirection, out hit, Mathf.Infinity,~gameManagerScript.manager.ShootThroughLayers))
        {
            buildableObjectScript hitBuildable = hit.collider.GetComponent<buildableObjectScript>();
            if (hitBuildable != null)
            {
                float amountToRepair = Mathf.Min(Mathf.Min(
                     hitBuildable.maxHealth - hitBuildable.currentHealth
                    ,repairAmount)
                    ,PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources * repairEfficiency);

                hitBuildable.currentHealth += amountToRepair;
                PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources -= (int) (amountToRepair / repairEfficiency);
                canRepair = false;
                PlayerHealthScript.CurrentPlayerHealthScript.StartCoroutine(setRepairingTrue());
            }
            
        }
    }

    IEnumerator setRepairingTrue()
    {
        yield return new WaitForSeconds(repairDelay);
        canRepair = true;
        yield return null;
    }
}
