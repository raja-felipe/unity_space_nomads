using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField] bool inDeath;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject generalPanel;
    [SerializeField] private GameObject gunPanel;
    [SerializeField] private GameObject enemyPanel;
    
    // General Display
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timerPercent;
    [SerializeField] private Image timerBar;

    // [SerializeField] private TextMeshProUGUI generalAccuracyText;
    [SerializeField] private TextMeshProUGUI generalDamageText;
    [SerializeField] private TextMeshProUGUI generalEnemiesText;
    
    // Gun Display
    private string primaryGunName;
    private string secondaryGunName;
    private const string ToolGunName = "ToolGun";
    private const string GrenadeName = "Grenade";
    
    [SerializeField] private Image primaryGunImage;
    [SerializeField] private TextMeshProUGUI primaryGunDamage;
    [SerializeField] private TextMeshProUGUI primaryGunShots;
    // [SerializeField] private TextMeshProUGUI primaryGunAccuracy;
    
    [SerializeField] private Image secondaryGunImage;
    [SerializeField] private TextMeshProUGUI secondaryGunDamage;
    [SerializeField] private TextMeshProUGUI secondaryGunShots;
    // [SerializeField] private TextMeshProUGUI secondaryGunAccuracy;
    
    [SerializeField] private Image grenadeImage;
    [SerializeField] private TextMeshProUGUI grenadeDamage;
    [SerializeField] private TextMeshProUGUI grenadeGunShots;
    // [SerializeField] private TextMeshProUGUI grenadeGunAccuracy;

    [SerializeField] Sprite grenadeStockSprite;

    [SerializeField] private Image toolGunImage;
    [SerializeField] private TextMeshProUGUI toolGunBuildings;
    
    // Enemy Display
    [SerializeField] private TextMeshProUGUI[] enemyTracker;
    [SerializeField] private Image[] enemyImages;
    private int MaxInGameEnemies = 9;
    [SerializeField] private float lerpSpeed = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        SetActiveGeneral(true);
        InitializeGunData();
        InitializeEnemyData();
        if (inDeath)
        {
            titleText.text = GlobalSceneManager.FailText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize the needed values
        GeneralTabUpdates();
        UpdateGunTab();
        UpdateEnemyCounts();
    }
    
    // Internal functions that render for the scores
    
    // Internal Functions that button functions run
    private void SetActiveGeneral(bool state)
    {
        generalPanel.SetActive(state);
        gunPanel.SetActive(gunPanel.activeSelf ? !state : gunPanel.activeSelf);
        enemyPanel.SetActive(enemyPanel.activeSelf ? !state : enemyPanel.activeSelf);
    }

    private void SetActiveGunPanel(bool state)
    {
        gunPanel.SetActive(state);
        generalPanel.SetActive(generalPanel.activeSelf ? !state : generalPanel.activeSelf);
        enemyPanel.SetActive(enemyPanel.activeSelf ? !state : enemyPanel.activeSelf);
    }

    private void SetActiveEnemyPanel(bool state)
    {
        enemyPanel.SetActive(state);
        generalPanel.SetActive(generalPanel.activeSelf ? !state : generalPanel.activeSelf);
        gunPanel.SetActive(gunPanel.activeSelf ? !state : gunPanel.activeSelf);
    }
    
    // External Functions for buttons
    public void GoToGeneral()
    {
        // Debug.Log("GOING TO GENERAL");
        SetActiveGeneral(!generalPanel.activeSelf);
        // inGeneral = !inGeneral;
    }

    public void GoToGun()
    {
        // Debug.Log("GOING TO GUNS");
        SetActiveGunPanel(!gunPanel.activeSelf);
        // inGuns = !inGuns;
    }

    public void GoToEnemy()
    {
        // Debug.Log("GOING TO ENEMIES");
        SetActiveEnemyPanel(!enemyPanel.activeSelf);
        // inEnemies = !inEnemies;
    }
    
    // External Functions for Editing the Different Scores
    private void InitializeGunData()
    {
        // Initialize the sprites
        primaryGunImage.sprite = GlobalSceneManager.SelectedGuns[0].gunSprite;
        secondaryGunImage.sprite = GlobalSceneManager.SelectedGuns[1].gunSprite;
        grenadeImage.sprite = grenadeStockSprite;
        toolGunImage.sprite = GlobalSceneManager.ToolGun.gunSprite;
        // Initialize the gun strings
        primaryGunName = GlobalSceneManager.SelectedGuns[0].gunName;
        secondaryGunName = GlobalSceneManager.SelectedGuns[1].gunName;
    }

    private void InitializeEnemyData()
    {
        // Main Idea here is to disable all the images and texts first, and only
        // enable them once enemies are
        for (int i = 0; i < MaxInGameEnemies; i++)
        {
            SetImageTransparency(enemyImages[i], true);
            SetTextTransparency(enemyTracker[i], true);
        }
    }

    private void SetImageTransparency(Image img, bool state)
    {
        img.color = state ? Color.clear : Color.white;
    }

    private void SetTextTransparency(TextMeshProUGUI txt, bool state)
    {
        txt.color = state ? Color.clear : Color.white;
    }
    
    
    // General Tab
    private void GeneralTabUpdates()
    {
        UpdateTimerUI();
        // UpdateGeneralAccuracy();
        UpdateGeneralDamage();
        UpdateGeneralEnemiesKilled();
    }
    
    private void UpdateTimerUI()
    {
        timerBar.fillAmount = GlobalSceneManager.EndTimeRatio;
        timerPercent.text = GlobalSceneManager.EndTimePercent;
        timerText.text = GlobalSceneManager.EndTime;
    }

    /*private void UpdateGeneralAccuracy()
    {
        float accuracy = 0f;
        
        if (GlobalSceneManager.ShotsHit.Count > 0 && GlobalSceneManager.TotalShots.Count > 0)
        {
            accuracy = (100*(GlobalSceneManager.ShotsHit.Sum(x => x.Value) / 
                                  GlobalSceneManager.TotalShots.Sum(x => x.Value)));
        }

        generalAccuracyText.text = "" + accuracy+"%";
    }*/

    private void UpdateGeneralDamage()
    {
        float damageDealt = 0;
        if (GlobalSceneManager.DamageDealt.Count > 0)
        {
            damageDealt = GlobalSceneManager.DamageDealt.Sum(x => x.Value);
        }

        generalDamageText.text = "" + (int)damageDealt;
    }

    private void UpdateGeneralEnemiesKilled()
    {
        int enemiesKilled = 0;
        if (GlobalSceneManager.KillCounts.Count > 0)
        {
            enemiesKilled = GlobalSceneManager.KillCounts.Sum(x => x.Value);
        }

        generalEnemiesText.text = "" + enemiesKilled;
    }
    
    // Gun Tab
    private void UpdateGunTab()
    {
        UpdatePrimaryWeapon();
        UpdateSecondaryWeapon();
        UpdateGrenade();
        UpdateToolGun();
    }
    
    private void UpdateWeapon(string weaponName, TextMeshProUGUI damageText, TextMeshProUGUI shotsText)
    {
        int shots;
        float hit;
        float damage;
        shots = GlobalSceneManager.TotalShots.TryGetValue(weaponName, out shots) ? shots : 0;
        hit = GlobalSceneManager.ShotsHit.TryGetValue(weaponName, out hit) ? hit : 0;
        damage = GlobalSceneManager.DamageDealt.TryGetValue(weaponName, out damage) ? damage : 0f;
        string shotInput = "" + shots;
        string hitInput = "" + hit;
        string damageInput = "" + damage;
        // accuracyText.text = shots > 0 ? ""+((hit/shots)*100) : ""+0;
        damageText.text = damageInput;
        shotsText.text = shotInput;
        /*Debug.Log(weaponName);
        Debug.Log("SHOTS "+shotInput);
        Debug.Log(GlobalSceneManager.TotalShots[weaponName]);*/
    }

    private void UpdatePrimaryWeapon()
    {
        UpdateWeapon(primaryGunName, primaryGunDamage, primaryGunShots);
        // UpdateWeapon(primaryGunName, primaryGunAccuracy, primaryGunDamage, primaryGunShots);
    }

    private void UpdateSecondaryWeapon()
    {
        UpdateWeapon(secondaryGunName, secondaryGunDamage, secondaryGunShots);
        // UpdateWeapon(secondaryGunName, secondaryGunAccuracy, secondaryGunDamage, secondaryGunShots);
    }

    private void UpdateGrenade()
    {
        UpdateWeapon(GrenadeName, grenadeDamage, grenadeGunShots);
        // UpdateWeapon(GrenadeName, grenadeGunAccuracy, grenadeDamage, grenadeGunShots);
    }

    private void UpdateToolGun()
    {
        toolGunBuildings.text = "" + GlobalSceneManager.BuildingsMade;
    }
    
    // Enemies Tab
    private void UpdateEnemyCounts()
    {
        if (GlobalSceneManager.KillCounts.Count > 0)
        {
            int i = 0;
            foreach (string enemyName in GlobalSceneManager.KillCounts.Keys)
            {
               // First set active the current image
               SetImageTransparency(enemyImages[i], false);
               SetTextTransparency(enemyTracker[i], false);
               
               // Now put in the values
               enemyImages[i].sprite = GlobalSceneManager.EnemyImageDict[enemyName];
               enemyTracker[i].text = "" + GlobalSceneManager.KillCounts[enemyName];
               i++;
            }
        }
    }
    
    // Use this to animate text and make it scroll rather than just pop
    private IEnumerator Animate(float targetVal, float defaultVal)
    {
        float current = targetVal = defaultVal;
        while (true)
        {
            current = Mathf.Lerp(current, targetVal, this.lerpSpeed);
            // Insert the functions you want to use here
            // UpdateText(Mathf.RoundToInt(current));

            yield return new WaitForSeconds(0.05f);
        }
    }
}


