using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBarImage.type = Image.Type.Filled;
        healthBarImage.fillAmount = 1.0f;
    }

    // Update is called once per frame
    private void UpdateHealthBar()
    {
        float currentHealth = PlayerHealthScript.CurrentPlayerHealthScript.health;
        float healthRatio = currentHealth / PlayerHealthScript.CurrentPlayerHealthScript.maxHealth;
        healthBarImage.fillAmount = healthRatio;
        // UpdateHudOnRender();
    }
}
