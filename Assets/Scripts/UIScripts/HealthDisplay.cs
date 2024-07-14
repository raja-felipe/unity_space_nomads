using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;
    private Color LOW_HEALTH = Color.red;
    private Color MEDIUM_HEALTH = Color.yellow;
    private Color HIGH_HEALTH = Color.white;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float currentHealth = PlayerHealthScript.CurrentPlayerHealthScript.health;
        float currentShields = PlayerHealthScript.CurrentPlayerHealthScript.shields;

        float healthRatio = currentHealth / PlayerHealthScript.CurrentPlayerHealthScript.maxHealth;
        float shieldRatio = currentShields / PlayerHealthScript.CurrentPlayerHealthScript.maxShields;

        // Change Color of Shields

        shieldText.text = "SHIELD: " + currentShields;
        healthText.text = "HEALTH: " + currentHealth;
    }
}
