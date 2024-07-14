using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class HudUiManager : MonoBehaviour
{
    public static HudUiManager HudManager;
    // Attributes associated with the script
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI clipText;
    [SerializeField] private TextMeshProUGUI grenadeText;
    [SerializeField] private TextMeshProUGUI gooText;
    [SerializeField] private TextMeshProUGUI gooGainedText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthHealedText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI interactableText;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private ObjectiveScript objective;
    [SerializeField] private ObjectiveScript pilotDoor;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI buildingDescText;
    [SerializeField] private TextMeshProUGUI buildingRotateText;
    [SerializeField] private TextMeshProUGUI buildingChangeText;
    // [SerializeField] private TextMeshProUGUI timerText;
    public CrosshairDisplay crosshairDisplay;
    
    private const int MinuteSeconds = 60;
    private const int DoubleDigits = 10;
    private Color LOW_HEALTH = Color.red;
    private Color MEDIUM_HEALTH = Color.yellow;
    private Color HIGH_HEALTH = Color.white;
    
    [SerializeField] private RenderTexture hudRenderTexture = null;
    [SerializeField] private Camera hudCamera = null;
    [SerializeField] private Material hudRenderMaterial = null;
    [SerializeField] private MeshRenderer hudMesh = null;
    
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private Image engineBar;
    [SerializeField] private Image pilotBar;
    [SerializeField] private Image[] gunImages;
    [SerializeField] private TextMeshProUGUI[] gunTexts;
    [SerializeField] private Image[] selectionImages;
    
    // Colors needed
    private Color healthColor = new Color(37f,221f, 24f);
    private Color shieldColor = new Color(26f, 207f, 194f);
    private Color objectiveColor = new Color(229f, 117f, 197f);
    private Color lowColor = new Color(255f, 0f, 0f);
    
    // Booleans needed to oscillate between red and normal color
    private bool healthRed = true;
    private bool shieldRed = false;
    private bool engineRed = false;
    private bool pilotDoorRed = false;
    private bool gunIconsActive = false;
    private const float gunIconDuration = 1.0f;
    private float gunDisplayCount = 0.0f;
    private int currentGunIndex = 0;
    
    // Variables for Coroutines
    private Color gainColor = new Color(0, 1, 0.1098f, 1);
    private Color lossColor = new Color(0.6902f, 0.0667f, 0.1333f, 1);
    private const float timePassed = 1.0f;
    private const float playerHealedDuration = 3.0f;
    private const float playerDamagedDuration = 3.0f;
    private const float gooGainedDuration = 3.0f;
    private float startingTransparency;
    
    // Start is called before the first frame update
    void Start()
    {
        interactableText.enabled = false;
        // Get the sizes of the images
        // GetBarParameters();
        HudManager = this;
        SetGunIcon();
        SetActiveIcons(false);
        // Do this to set the color to transparent before doing anything
        startingTransparency = healthHealedText.color.a;
        Color currColor = healthHealedText.color;
        currColor.a = 0.0f;
        healthHealedText.color = currColor;
        currColor = gooGainedText.color;
        currColor.a = 0.0f;
        gooGainedText.color = currColor;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
        UpdateGunIconDisplayStatus();
        UpdateBuildings();
        UpdatePlayerHealth();
        UpdateShieldHealth();
        // UpdateObjectiveHealth();
        UpdateInteractable();
        // UpdateTimer();
        UpdateHealthBar();
        UpdateShieldBar();
        UpdateObjectiveBar();
        UpdatePilotDoorBar();
        UpdateHudOnRender();
    }
    
    // Update the current gun icon
    private void SetGunIcon()
    {
        for (int i=0; i<PlayerGunScript.currentGunScript.equippedGuns.Length; i++)
        {
            GunData data = PlayerGunScript.currentGunScript.equippedGuns[i].data;
            Image gunImg = gunImages[i];
            gunImg.sprite = data.gunSprite;
        }
    }
    
    // Use this to activate the gun displays
    private void SetActiveIcons(bool state)
    {
        gunIconsActive = state;
        gunDisplayCount = gunIconsActive ? 0f : gunDisplayCount;
        foreach (Image img in gunImages)
        {
            img.transform.gameObject.SetActive(state);
        }

        for (int i=0; i<gunTexts.Length; i++)
        {
            gunTexts[i].text = state ? ""+ (i + 1) : "";
            selectionImages[i].transform.gameObject.SetActive(state && i == currentGunIndex);
        }
    }
    
    // Need this function to call when switching guns
    public void UpdateGunIconDisplayStatus()
    {
        // Need to first check if the weapon was switched
        if (currentGunIndex != PlayerGunScript.currentGunScript.currentEquipped)
        {
            currentGunIndex = PlayerGunScript.currentGunScript.currentEquipped;
            // When switched, start the timer
            SetActiveIcons(true);
            return;
        }

        if (gunDisplayCount >= gunIconDuration)
        {
            gunDisplayCount = 0.0f;
            SetActiveIcons(false);
            return;
        }

        // Here we need to set the timer
        if (gunIconsActive)
        {
            gunDisplayCount += Time.deltaTime;
        }
    }
    
    // Update various values shown in the UI
    private void UpdateAmmo()
    {
        bool isMelee = (PlayerGunScript.currentGunScript.currentGun.data.gunName == "Hammer Time" |
                         PlayerGunScript.currentGunScript.currentGun.data.gunName == "ToolGun");
        
        int ammo = PlayerGunScript.currentGunScript.currentGun.currentAmmo;
        int clip = PlayerGunScript.currentGunScript.currentGun.currentClip;
        int grenades = PlayerGunScript.currentGunScript.grenadeCount;
        int goo = PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources;

        ammoText.text = isMelee ? "" : "" + ammo;
        clipText.text = isMelee ? "" : "" + clip;
        gooText.text = "" + goo;
        grenadeText.text = "" + grenades;
        // UpdateHudOnRender();
    }

    private void UpdateBuildings()
    {
        bool isToolGun = PlayerGunScript.currentGunScript.currentGun.data.gunName.Equals("ToolGun");

        buildingNameText.text =
            isToolGun
                ? gameManagerScript.manager.buildingDatas[
                        ((ToolGunData)(PlayerGunScript.currentGunScript.currentGun.data)).currentBuildingSelected]
                    .buildingName : "";
        buildingDescText.text =
            isToolGun
                ? gameManagerScript.manager.buildingDatas[
                        ((ToolGunData)(PlayerGunScript.currentGunScript.currentGun.data)).currentBuildingSelected]
                    .buildingDescription : "";
        buildingRotateText.text = isToolGun ? 
            $"{GlobalSceneManager.inputLabelList[GlobalSceneManager.Reload]} TO ROTATE BUILDING": "";
        buildingChangeText.text = isToolGun ? 
            $"{GlobalSceneManager.inputLabelList[GlobalSceneManager.AltFire]} TO CHANGE BUILDING": "";
    }
    
    private void UpdatePlayerHealth()
    {
        float currentHealth = PlayerHealthScript.CurrentPlayerHealthScript.health;
        float healthRatio = currentHealth / PlayerHealthScript.CurrentPlayerHealthScript.maxHealth;
        healthText.text = "HEALTH: " + (int)(currentHealth);
        // UpdateHudOnRender();
    }

    private void UpdateHealthBar()
    {
        float currentHealth = PlayerHealthScript.CurrentPlayerHealthScript.health;
        float healthRatio = currentHealth / PlayerHealthScript.CurrentPlayerHealthScript.maxHealth;
        healthBar.fillAmount = healthRatio;
        
        if (healthRatio < 0.0f)
        {
            // First determine if the color should be red or not
            // Color currColor = healthBar.color;
            
            /*Debug.Log(string.Format("HEALTH: {0}, {1}, {2}", (int)healthColor.r, (int)healthColor.g, (int)healthColor.b));
            Debug.Log(string.Format("LOW HEALTH: {0}, {1}, {2}", (int)lowColor.r, (int)lowColor.g, (int)lowColor.b));
            Debug.Log(string.Format("CURR COLOR: {0}, {1}, {2}", (int)currColor.r, (int)currColor.g, (int)currColor.b));*/
            
            // We need to flash the health bar red in this case
            Color targetColor = healthRed ? lowColor : healthColor;
            Color imgColor = healthBar.color;
            imgColor.r = Mathf.Lerp(imgColor.r, targetColor.r, Time.deltaTime);
            imgColor.b = Mathf.Lerp(imgColor.b, targetColor.b, Time.deltaTime);
            imgColor.g = Mathf.Lerp(imgColor.g, targetColor.g, Time.deltaTime);
            healthBar.color = imgColor;
            /*if (healthRed)
            {
                Debug.Log("TURNING RED");
                currColor = Color.Lerp(currColor, lowColor, Time.deltaTime);
            }

            else
            {
                Debug.Log("TURNING GREEN");
                currColor = Color.Lerp(currColor, healthColor, Time.deltaTime);
            }*/
            // healthBar.color = currColor;

            if (healthBar.color == targetColor)
            {
                healthRed = !healthRed;
                Debug.Log("COLOR CHANGE: "+ (healthRed ? lowColor : healthColor));
            }
        }

        else
        {
            Color currColor = healthBar.color;
            currColor.r = healthBar.color.r;
            currColor.g = healthBar.color.g;
            currColor.b = healthBar.color.b;
            healthBar.color = currColor;
            healthRed = false;
        }
        // UpdateHudOnRender();
    }

    private void UpdateShieldHealth()
    {
        float currentShields = PlayerHealthScript.CurrentPlayerHealthScript.shields;
        float shieldRatio = currentShields / PlayerHealthScript.CurrentPlayerHealthScript.maxShields;
        shieldText.text = "SHIELD: " + (int)(currentShields);
        // UpdateHudOnRender();
    }

    private void UpdateShieldBar()
    {
        float currentShields = PlayerHealthScript.CurrentPlayerHealthScript.shields;
        float shieldRatio = currentShields / PlayerHealthScript.CurrentPlayerHealthScript.maxShields;
        shieldBar.fillAmount = shieldRatio;
        // UpdateHudOnRender();
    }

    private void UpdateObjectiveHealth()
    {
        objectiveText.text = "ENGINE: " + objective.currentHealth + "/" + objective.maxHealth;
        // UpdateHudOnRender();
    }

    private void UpdateObjectiveBar()
    {
        float objectiveRatio = objective.currentHealth / objective.maxHealth;
        engineBar.fillAmount = objectiveRatio;
    }

    private void UpdatePilotDoorBar()
    {
        float objectiveRatio = pilotDoor.currentHealth / pilotDoor.maxHealth;
        pilotBar.fillAmount = objectiveRatio;
    }

    private void UpdateInteractable()
    {
        // Get the positions of the player and interactable objects
        Vector3 cameraFacing = playerCamera.transform.forward;
        Vector3 playerPos = playerCamera.transform.position;
        RaycastHit checkInteract;
        
        // Check if there is a hit on any of the interactables
        // ASSUME none of these overlap
        if (Physics.Raycast(playerPos, cameraFacing, out checkInteract, Mathf.Infinity))
        {
            // Check first if the components have an interaction text
            string displayText = null;
            
            // Debug.Log(checkInteract.collider.tag);
            
            if (checkInteract.collider.tag == InteractableData.Tag)
            {
                displayText = checkInteract.collider.GetComponent<OnInteractDisplay>().GetDisplayString();
            }
            
            // Dislpay the interaction texts
            if (displayText != null)
            {
                interactableText.text = displayText;
                interactableText.enabled = true;
            }

            else
            {
                interactableText.enabled = false;
            }
        }
        // UpdateHudOnRender();
    }

    /*private void UpdateTimer()
    {
        float timeLeft = gameManager.getTargetTime() - Time.deltaTime;
        int minutesLeft = (int)(timeLeft / MinuteSeconds);
        int secondsLeft = (int)(timeLeft % MinuteSeconds);
        string minutesDisplay = "";
        string secondsDisplay = "";
        
        // Convert minutes to string
        if (minutesLeft >= DoubleDigits)
        {
            minutesDisplay = minutesLeft.ToString();
        }

        else
        {
            minutesDisplay = minutesLeft.ToString("00");
        }

        // Convert seconds to string
        if (secondsLeft >= DoubleDigits)
        {
            secondsDisplay = secondsLeft.ToString();
        }

        else
        {
            secondsDisplay = secondsLeft.ToString("00");
        }
        
        // Now display the time
        timerText.text = minutesDisplay + ":" + secondsDisplay;
        // UpdateHudOnRender();
    }*/
    
    // Public Function to call each time the values update
    private void UpdateHudOnRender()
    {
        // ResetBars();
        UpdateHudOnRenderTexture();
        UpdateHudMaterialWithRenderTexture();
        UpdateMeshMaterial();
    }
    
    // Need these functions to update the HUD each time a value changes
    private void UpdateHudOnRenderTexture()
    {
        // This outputs the updated render texture that occurs
        // when values change in the HUD
        hudCamera.targetTexture.Release(); // Reset the texture here
        hudCamera.targetTexture = hudRenderTexture;
    }
    
    // Use this function to reset the material after an update occurs
    private void UpdateHudMaterialWithRenderTexture()
    {
        hudRenderMaterial.SetTexture("_MainTex", hudRenderTexture);
    }

    private void UpdateMeshMaterial()
    {
        // Now reassign the renderer
        if (hudMesh != null && hudRenderMaterial != null)
        {
            hudMesh.material = null;
            hudMesh.material = hudRenderMaterial;
        }
        else
        {
            Debug.Log("MATERIAL NOT WORKING");
        }
    }
    
    // Render the health restored
    public void ShowHealthChange(float amount, bool gainedHealth)
    {
        // Set the heal amount
        healthHealedText.color = gainedHealth ? gainColor : lossColor;
        string gainSymbol = gainedHealth ? "+" : "-";
        healthHealedText.text = gainSymbol + amount;
        StartCoroutine(PlayerHealthRoutine());
    }
    
    private IEnumerator PlayerHealthRoutine()
    {
        float remainingTime = playerHealedDuration;
        Color startColor = healthHealedText.color;
        startColor.a = startingTransparency;
        healthHealedText.color = startColor;
        while (remainingTime > 0)
        {
            Color currColor = healthHealedText.color;
            currColor.a *= remainingTime / playerHealedDuration;
            healthHealedText.color = currColor;
            yield return new WaitForSeconds(timePassed);
            remainingTime -= timePassed;
        }
        yield return null;
    }
    
    // Use this to report goo
    public void ShowGooChange(float amount, bool gainedGoo)
    {
        gooGainedText.color = gainedGoo ? gainColor : lossColor;
        string gainSymbol = gainedGoo ? "+" : "-";
        gooGainedText.text = gainSymbol + amount;
        StartCoroutine(PlayerGooRoutine());
    }

    private IEnumerator PlayerGooRoutine()
    {
        float remainingTime = gooGainedDuration;
        Color startColor = gooGainedText.color;
        startColor.a = startingTransparency;
        gooGainedText.color = startColor;
        while (remainingTime > 0)
        {
            Color currColor = gooGainedText.color;
            currColor.a *= remainingTime / gooGainedDuration;
            gooGainedText.color = currColor;
            yield return new WaitForSeconds(timePassed);
            remainingTime -= timePassed;
        }
    }
    
    // Use this to flash the health each time you get hit
    private void FlashHitEffect(Image bar)
    {
        
    }
        
    // Any getters for elements that need to be accessed outside this class
    public Image GetHealthBar()
    {
        return healthBar;
    }

    public Image GetShieldBar()
    {
        return shieldBar;
    }

    public Image GetEngineBar()
    {
        return engineBar;
    }

    public Image GetPilotBar()
    {
        return pilotBar;
    }
}
