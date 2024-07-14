using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public abstract class PlayerCanHit : MonoBehaviour
{
    /// <summary>
    /// This interface is put on anything the enemies can damage.
    /// "damage" returns true if damage was done.
    /// </summary>
    /// 
    public abstract float damage(float amount, GameObject source);

    public float currentHealth = 0;
    public float maxHealth = 1;
    public abstract void knockback(Vector3 direction, float duration);
    
    public Image enemyHealthBar;
    public Image enemyHealthBackground;
    public Camera cameraToFace;
    private const float barDuration = 1.5f;
    private float currBarTime = 0.0f;
    private bool isDisplayingHealth = false;
    [SerializeField] private float healthBarHeight;

    public void doOnAwake()
    {
        
        cameraToFace = PlayerControlScript.currentPlayer.playerCamera;
        SetActiveEnemyHealth(isDisplayingHealth);
    }

    public void doOnUpdate()
    {
        
        UpdateDisplayHealthBar();
    }
    protected void SetActiveEnemyHealth(bool state)
    {
        if (enemyHealthBar != null)
        {
            isDisplayingHealth = state;
            currBarTime = 0.0f;
            // Set the healthbars to active
            enemyHealthBar.transform.gameObject.SetActive(state);
            enemyHealthBackground.transform.gameObject.SetActive(state);   
        }
    }
    
    // Helper Function to display healthbar on screen
    protected void DisplayHealthBar()
    {
        SetActiveEnemyHealth(true);
    }
    protected void UpdateDisplayHealthBar()
    {
        // Remove this line later after debugging
        if (enemyHealthBar != null)
        {
            if (isDisplayingHealth)
            {
                // Disable the healthbar if duration is up
                if (currBarTime >= barDuration)
                {
                    SetActiveEnemyHealth(false);
                    return;
                }
                currBarTime+=Time.deltaTime;

                // Now update the healthbar fillamount
                float healthRatio = currentHealth / maxHealth;
                enemyHealthBar.fillAmount = healthRatio;
                
                // Get the bar position to align with the enemy
                Vector3 healthBarPos = new Vector3(0, 0, 0);
                healthBarPos.x = transform.position.x;
                healthBarPos.z = transform.position.z;
                healthBarPos.y = transform.position.y + healthBarHeight;
                enemyHealthBackground.transform.parent.gameObject.GetComponent<RectTransform>().LookAt(cameraToFace.transform);
                enemyHealthBackground.transform.parent.gameObject.GetComponent<RectTransform>().position = healthBarPos;
            }   
        }
    }
}
