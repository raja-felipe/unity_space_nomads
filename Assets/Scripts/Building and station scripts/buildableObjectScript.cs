using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class buildableObjectScript : MonoBehaviour , EnemyCanHit, IsInteractable
{
    public float maxHealth;
    public NavMeshModifierVolume thisModifier;
    public float currentHealth;
    public Vector3 BuildingOffset;
    public int cost;
    public float RotationAdd;
    // Use these for UI rendering
    public string buildingName;
    public string buildingDescription;

    public bool isTall;
    public bool groundBuilding; //floor spikes building type
    // Start is called before the first frame update
    public virtual void Start()
    {
        thisModifier = GetComponentInChildren<NavMeshModifierVolume>();
        if (thisModifier != null)
        {
            navMeshManager.queueRebake();
        }

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
    public float damage(float amount, EnemyControlScript source)
    {
        float prevHealth = currentHealth;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            source.onKill(this);
            die();
            currentHealth = 0;
        }
        return prevHealth - currentHealth;
    }

    public virtual void die()
    {

        if (thisModifier != null)
        {
            gameManagerScript.manager.removeFromNavMesh(thisModifier);
            navMeshManager.queueRebake();
        }
        Destroy(this.gameObject);
    }
    
    public bool isDead()
    {
        return currentHealth < 0;
    }

    public void Interact()
    {
        PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources += (int) (cost * currentHealth / (2 * maxHealth));
        die();
    }
    
    public string WriteInteractableText()
    {
        string interactionButton = "E";
        int gooReturned = (int)(cost * currentHealth / (2 * maxHealth));
        return string.Format("Press {0} to destroy building and regain {1} goo", interactionButton, gooReturned);
    }

    public void TriggerInteraction()
    {
        Interact();
    }
}
