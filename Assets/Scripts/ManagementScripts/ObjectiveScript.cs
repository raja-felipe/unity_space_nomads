using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour, EnemyCanHit
{
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private string objectiveName;

    [SerializeField] public GameUIManager uiManager;
    public void Start()
    {
        uiManager = PlayerHealthScript.CurrentPlayerHealthScript.uiManager;
        currentHealth = maxHealth;
    }

    public float damage(float amount, EnemyControlScript source)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            
            source.onKill(this);
            die();
        }

        return amount;
    }

    public void die()
    {
        uiManager.GameOver($"{objectiveName.ToUpper()} WAS DESTROYED");
    }

    public bool isDead()
    {
        return currentHealth < 0;
    }
}
