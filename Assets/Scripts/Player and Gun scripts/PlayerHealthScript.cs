using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour , EnemyCanHit
{
    /// <summary>
    ///
    /// TODO:
    /// Death.
    /// </summary>
    public float maxHealth = 100;
    public float maxShields = 100;
    public float health = 100;
    public float shields = 100;
    public static PlayerHealthScript CurrentPlayerHealthScript;
    [SerializeField] public GameUIManager uiManager;

    public int currentOwnedResources = 0;

    void Awake()
    {
        CurrentPlayerHealthScript = this;
    }

    // return true if damage was done.
    public float damage(float amount, EnemyControlScript source)
    {
        Vector3 damagePositionDifference = this.gameObject.transform.position - source.gameObject.transform.position;
        Vector2 damagePosDiffFlat = new Vector2(damagePositionDifference.x, damagePositionDifference.z).normalized;
        Vector2 forwardRotationFlat = new Vector2(transform.forward.x, transform.forward.z);
        float rotationToEnemyAngle = Vector2.SignedAngle(damagePosDiffFlat, forwardRotationFlat);
        uiManager.spawnPlayerHitMarker(rotationToEnemyAngle);
        float damageCarryOver = amount;
        if (amount <= 0)
        {
            return 0f;
        }
        float damageDealt = 0;
        if (shields > 0)
        {
            //shield damage triggers go here.
            shields = shields - amount;
            damageDealt += amount;
            damageCarryOver = -shields;
            if (shields < 0)
            {
                damageDealt -= damageCarryOver;
                shields = 0;
                //shield break audio?
            }
        }
        if (damageCarryOver > 0)
        {
            //health damage triggers go here.
            health -= damageCarryOver;
            damageDealt += damageCarryOver;
            if (health <= 0)
            {
                
                source.onKill(this);
                Die();
            }
        }

        HudUiManager.HudManager.ShowHealthChange(damageDealt, false);
        
        GlobalSceneManager.AddDamageTaken(damageDealt);
        return damageDealt;
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Min(maxHealth, health);
    }

    private void Die()
    {
        uiManager.GameOver("YOU DIED");
    }
    
    public bool isDead()
    {
        return health < 0;
    }
}
