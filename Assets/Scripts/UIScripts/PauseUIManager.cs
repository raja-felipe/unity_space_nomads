using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private gameManagerScript manager;
    [SerializeField] private GameObject statsCanvas;
    [SerializeField] private GameObject controlsCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TimerDisplay timerDisplay;
    // Several Text and Image Fields to fill in
    [SerializeField] private ObjectiveScript objective;
    [SerializeField] private ObjectiveScript pilotDoor;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private Image objectiveBar;
    [SerializeField] private Image pilotBar;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currHealthText;
    [SerializeField] private TextMeshProUGUI currShieldText;
    [SerializeField] private TextMeshProUGUI currObjectiveText;
    [SerializeField] private TextMeshProUGUI currPilotText;
    /*[SerializeField] private Image primaryImg;
    [SerializeField] private Image secondaryImg;
    [SerializeField] private Image toolGunImg;*/
    /*[SerializeField] private TextMeshProUGUI primaryTxt;
    [SerializeField] private TextMeshProUGUI secondaryTxt;
    [SerializeField] private TextMeshProUGUI toolGunTxt;*/
    public TextMeshProUGUI[] gunTexts;
    public Image[] gunImages;
    // [SerializeField] private GameObject gunParent;

    private const string SettingsString = "SETTINGS";
    private const string ControlsString = "CONTROLS";
    private const string StatsString = "STATS";
    private bool inPause = false;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        GoToStats();
        // InitializeGunAttributes();
        //InitializeGunImages();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && !inPause)
        {
            GoToStats();
            inPause = !inPause;
        }
        
        else if (Input.GetKeyDown(KeyCode.Tab) && inPause)
        {
            DisableCanvases();
            inPause = !inPause;
        }
        UpdateBars();
        UpdateTimer();
        UpdateGuns();
    }
    
    // Functions to constantly use when displaying various information OR
    // initialize information
    /*public void InitializeGunImages()
    {
        const int imageIndex = 0;
        int weaponsLength = gunParent.transform.childCount;
        for (int i = 0; i < weaponsLength; i++)
        {
            gunImages[i] = gunParent.transform.GetChild(i).gameObject.transform.GetChild(imageIndex).GetComponent<Image>();
        }
    }*/
    
    public void InitializeGunAttributes()
    {
        /*gunTexts = new TextMeshProUGUI[PlayerGunScript.currentGunScript.equippedGuns.Length];
        gunImages = new Image[PlayerGunScript.currentGunScript.equippedGuns.Length];*/
        UpdateGuns();
    }
    
    public void UpdateBars()
    {
        // Update Health
        float healthRatio = PlayerHealthScript.CurrentPlayerHealthScript.health /
                            PlayerHealthScript.CurrentPlayerHealthScript.maxHealth;
        int currHealth = (int)(PlayerHealthScript.CurrentPlayerHealthScript.health);
        healthBar.fillAmount = healthRatio;
        currHealthText.text = "" + currHealth;
        
        // Update Shields
        float shieldRatio = PlayerHealthScript.CurrentPlayerHealthScript.shields /
                            PlayerHealthScript.CurrentPlayerHealthScript.maxShields;
        int currShields = (int)(PlayerHealthScript.CurrentPlayerHealthScript.shields);
        shieldBar.fillAmount = shieldRatio;
        currShieldText.text = "" + currShields;
        
        // Update Objective Health
        float objectiveRatio = objective.currentHealth / objective.maxHealth;
        int currObjective = (int)(objective.currentHealth);
        objectiveBar.fillAmount = objectiveRatio;
        currObjectiveText.text = "" + currObjective;
        
        // Update Objective Health
        float pilotRatio = pilotDoor.currentHealth / pilotDoor.maxHealth;
        int currPilotDoor = (int)(pilotDoor.currentHealth);
        pilotBar.fillAmount = pilotRatio;
        currPilotText.text = "" + currPilotDoor;
    }

    public void UpdateTimer()
    {
        timerText.text = timerDisplay.currTime;
    }

    public void UpdateGuns()
    {
        for (int i = 0; i < gunImages.Length; i++)
        {
            UpdateSpecificGun(i);
        }
    }

    public void UpdateSpecificGun(int i)
    {
        gunImages[i].sprite = PlayerGunScript.currentGunScript.equippedGuns[i].data.gunSprite;
        gunTexts[i].text = string.Format("{0:00}|{1:00}", PlayerGunScript.currentGunScript.equippedGuns[i].currentClip,
            PlayerGunScript.currentGunScript.equippedGuns[i].currentAmmo);
    }
    
    // Functions to swtich canvases
    public void SetActivePauseCanvas(bool statState, bool settingState, bool controlState)
    {
        statsCanvas.SetActive(statState);
        settingsCanvas.SetActive(settingState);
        controlsCanvas.SetActive(controlState);

        if (controlState) buttonText.text = StatsString;
        else buttonText.text = ControlsString;

    }
    
    // External Functions Accessed by Button/s
    public void GoToStats()
    {
        SetActivePauseCanvas(true, false, false);
    }

    public void GoToSettings()
    {
        SetActivePauseCanvas(false, true, false);
    }

    public void GoToControls()
    {
        SetActivePauseCanvas(false, false, true);
    }

    public void DisableCanvases()
    {
        SetActivePauseCanvas(false, false, false);
    }

    public void SwitchBetweenControlAndStats()
    {
        if (settingsCanvas.activeSelf)
        {
            if (buttonText.text.Equals(StatsString)) GoToStats();
            else if (buttonText.text.Equals(ControlsString)) GoToControls();
        }
        else
        {
            SetActivePauseCanvas(!statsCanvas.activeSelf, false, !controlsCanvas.activeSelf);
        }
    }
}
