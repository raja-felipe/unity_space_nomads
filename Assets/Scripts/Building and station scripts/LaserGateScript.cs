using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

public class LaserGateScript : buildableObjectScript
{
    public float cooldown;
    public float dealDamage;
    public float totalUses;
    public float currentTimesUsed;
    public bool desiredDeployState= true;
    public bool deployed = false;
    public Animation anims;
    public AnimationClip deployAnim;
    public AnimationClip unDeployAnim;
    public ParticleSystem sparkEffect;
    public OnInteractDisplay[] laserGateParts;
    private bool allAssigned = false;
    public override void Start()
    {
        base.Start();
        anims = GetComponent<Animation>();
        StartCoroutine(DeployRoutine());
        foreach (OnInteractDisplay part in laserGateParts)
        {
            part.interactableObject = GetComponent<IsInteractable>();
        }
    }

    public override void Update()
    {
        AdditionalInteractableAssignments();
    }
    
    private void AdditionalInteractableAssignments()
    {
        if (allAssigned) return;
        
        bool isAllAssigned = true;
        
        foreach (OnInteractDisplay part in laserGateParts)
        {
            if (part.interactableObject == null)
            {
                part.interactableObject = GetComponent<IsInteractable>();
                isAllAssigned = false;
            }
            // Debug.Log(part.interactableObject != null);
        }

        allAssigned = isAllAssigned;
    }

    public void OnTriggerStay(Collider other)
    {
        
        if (deployed)
        {
            // Debug.Log(other.gameObject.name);
            if ((LayerMask.GetMask("Enemy") & (1 <<  other.transform.root.gameObject.layer)) != 0)
            {
                PlayerCanHit hitEnemy = other.GetComponent<Collider>().transform.root.GetComponent<PlayerCanHit>();
                if (hitEnemy == null)
                {
                    Debug.Log("Enemy with no controller");
                    return;
                }

                hitEnemy.damage(dealDamage, this.gameObject);
                StartCoroutine(doCooldown());
                currentTimesUsed++;
            }

        }

        if (currentTimesUsed >= totalUses)
        {
            die();
        }
    }

    public IEnumerator doCooldown()
    {
        
        desiredDeployState = false;
        yield return new WaitForSeconds(cooldown);
        desiredDeployState = true;
    }
    public IEnumerator DeployRoutine()
    {
        while (this.enabled)
        {
            if (deployed != desiredDeployState)
            {
                if (desiredDeployState)
                {
                    anims.Play(deployAnim.name);
                    yield return new WaitForSeconds(deployAnim.length);
                    deployed = true;
                }
                else
                {
                    deployed = false;
                    anims.Play(unDeployAnim.name);
                    yield return new WaitForSeconds(unDeployAnim.length);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public override void die()
    {
        base.die();
        
    }
}
