using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStationScript : MonoBehaviour, EnemyCanHit, IsInteractable
{
    // Start is called before the first frame update
    public ShipStationData data;
    public float currentCooldownTimer;
    public int currentUses;
    public float useCooldownTimer; // we don't want the player to accidentally use all the uses in one go.
    public AudioSource thisAudioSource;
    public float currentHealth = 200;
    public OnInteractDisplay displayObject;
    public OnInteractDisplay[] shipStationParts;
    public bool allAssigned = false;
    void Start()
    {
        currentUses = data.maxUses;
        currentHealth = data.maxHealth;
        foreach (OnInteractDisplay part in shipStationParts)
        {
            part.interactableObject = GetComponent<IsInteractable>();
            // Debug.Log(part.interactableObject != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        useCooldownTimer += Time.deltaTime;
        if (currentUses < data.maxUses)
        {
            
            currentCooldownTimer += Time.deltaTime;
            if (currentCooldownTimer > data.useRechargeTime)
            {
                currentUses += 1;
                currentCooldownTimer = 0;
            }
            
        }
        // Update the interactable text
        UpdateInteractText();
        AdditionalsInteractableAssignments();
    }

    private void AdditionalsInteractableAssignments()
    {
        if (allAssigned) return;
        
        bool isAllAssigned = true;
        
        foreach (OnInteractDisplay part in shipStationParts)
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

    public void Interact()
    {
        if (thisAudioSource != null)
        {
            thisAudioSource.Play();
        }
        if (currentUses > 0 && useCooldownTimer > data.useCooldown)
        {
            data.onInteract(this.gameObject);
            currentUses -= 1;
            useCooldownTimer = 0;
        }
    }

    public float damage(float amount, EnemyControlScript source)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Destroy(this.gameObject);
        }
        return amount;
    }
    
    // Helper Function to update the interaction text
    public void UpdateInteractText()
    {
        data.UpdateDisplayString(currentUses, currentCooldownTimer);
        displayObject.SetDisplayString(data.displayString);
    }

    public string WriteInteractableText()
    {
        return data.displayString;
    }

    public void TriggerInteraction()
    {
        Interact();
    }
}
