using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float volumeMin;
    private float volumeMax;
    
    [SerializeField] private Slider gunVolumeSlider;
    [SerializeField] private TMP_InputField gunVolumeInput;
    [SerializeField] private TextMeshProUGUI gunVolumeText;
    private string defaultGunVolumeText;
    private string defaultGunVolumeErrorText;
    
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TMP_InputField musicVolumeInput;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    private string defaultMusicVolumeText;
    private string defaultMusicVolumeErrorText;
    
    [SerializeField] private Slider enemyVolumeSlider;
    [SerializeField] private TMP_InputField enemyVolumeInput;
    [SerializeField] private TextMeshProUGUI enemyVolumeText;
    private string defaultEnemyVolumeText;
    private string defaultEnemyVolumeErrorText;
    
    [SerializeField] private Slider dialogueVolumeSlider;
    [SerializeField] private TMP_InputField dialogueVolumeInput;
    [SerializeField] private TextMeshProUGUI dialogueVolumeText;
    private string defaultDialogueVolumeText;
    private string defaultDialogueVolumeErrorText;
    
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_InputField sensitivityInput;
    [SerializeField] private TextMeshProUGUI sensitivityText;
    private string defaultSensitivityText;
    private string defaultSensitivityErrorText;
    private float sensitivityMin;
    private float sensitivityMax;
    
    [SerializeField] private Slider healingSlider;
    [SerializeField] private TMP_InputField healingInput;
    [SerializeField] private TextMeshProUGUI healingText;
    private string defaultHealingText;
    private string defaultHealingErrorText;
    private float healingMin;
    private float healingMax;
    
    /*[SerializeField] private Slider difficultySlider;
    [SerializeField] private TextMeshProUGUI difficultyText;
    private string[] difficultyNames = { "EASY", "MEDIUM", "HARD" };*/
    
    // Add more options stuff if ever
    // Add settings constants if ever
    
    
    void Start()
    {
        // Do this to set default settings if first time
        if (GlobalSceneManager.firstTime)
        {
            GlobalSceneManager.firstTime = false;
            GlobalSceneManager.manager.ApplyDefaultSettings();
        }
        InitializeSliderValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Initialize the slider values
    private void InitializeSliderValues()
    {
        // Volume
        volumeMin = GlobalSceneManager.VolumeMin;
        volumeMax = GlobalSceneManager.VolumeMax;
        
        // Set the listeners
        gunVolumeSlider.onValueChanged.AddListener(SetGunVolume);
        gunVolumeInput.onEndEdit.AddListener(SetGunVolumeInput);
        
        enemyVolumeSlider.onValueChanged.AddListener(SetEnemyVolume);
        enemyVolumeInput.onEndEdit.AddListener(SetEnemyVolumeInput);
        
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        musicVolumeInput.onEndEdit.AddListener(SetMusicVolumeInput);
        
        dialogueVolumeSlider.onValueChanged.AddListener(SetDialogueVolume);
        dialogueVolumeInput.onEndEdit.AddListener(SetDialogueVolumeInput);
        
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        sensitivityInput.onEndEdit.AddListener(SetSensitivityInput);
        // difficultySlider.onValueChanged.AddListener(SetDifficulty);
        
        healingSlider.onValueChanged.AddListener(SetHealing);
        healingInput.onEndEdit.AddListener(SetHealingInput);
        
        // Enemy Volume
        enemyVolumeSlider.minValue = volumeMin;
        enemyVolumeSlider.maxValue = volumeMax;
        defaultEnemyVolumeText = string.Format("Set volume value from {0} to {1}", volumeMin, 
            volumeMax);
        defaultEnemyVolumeErrorText = string.Format("Value not in range of {0}-{1}", volumeMin, volumeMax);
        enemyVolumeText.text = defaultEnemyVolumeText;
        enemyVolumeSlider.value = GlobalSceneManager.EnemyVolume;
        enemyVolumeInput.text = "" + GlobalSceneManager.EnemyVolume;
        
        // Healing
        healingMin = GlobalSceneManager.HealingMin;
        healingMax = GlobalSceneManager.HealingMax;
        healingSlider.minValue = healingMin;
        healingSlider.maxValue = healingMax;
        defaultHealingText = string.Format("Set healing value from {0} to {1}", healingMin, healingMax);
        defaultHealingErrorText = string.Format("Value not in range of {0}-{1}", healingMin, healingMax);
        healingSlider.value = GlobalSceneManager.Healing;
        healingText.text = defaultHealingText;
        healingInput.text = "" + GlobalSceneManager.Healing;
        
        // Sensitivity
        sensitivityMin = GlobalSceneManager.SensitivityMin;
        sensitivityMax = GlobalSceneManager.SensitivityMax;
        sensitivitySlider.minValue = sensitivityMin;
        sensitivitySlider.maxValue = sensitivityMax;
        defaultSensitivityText = string.Format("Set sensitivity value from {0} to {1}", sensitivityMin, 
            sensitivityMax);
        defaultSensitivityErrorText = string.Format("Value not in range of {0}-{1}", sensitivityMin, sensitivityMax);
        sensitivityText.text = defaultSensitivityText;
        sensitivitySlider.value = GlobalSceneManager.SelectedSensitivity;
        sensitivityInput.text = "" + GlobalSceneManager.SelectedSensitivity;
        
        // Gun Volume
        gunVolumeSlider.minValue = volumeMin;
        gunVolumeSlider.maxValue = volumeMax;
        defaultGunVolumeText = string.Format("Set volume value from {0} to {1}", volumeMin, 
            volumeMax);
        defaultGunVolumeErrorText = string.Format("Value not in range of {0}-{1}", volumeMin, volumeMax);
        gunVolumeText.text = defaultGunVolumeText;
        gunVolumeSlider.value = GlobalSceneManager.GunVolume;
        gunVolumeInput.text = "" + GlobalSceneManager.GunVolume;
        
        // Music Volume
        musicVolumeSlider.minValue = volumeMin;
        musicVolumeSlider.maxValue = volumeMax;
        defaultMusicVolumeText = string.Format("Set volume value from {0} to {1}", volumeMin, 
            volumeMax);
        defaultMusicVolumeErrorText = string.Format("Value not in range of {0}-{1}", volumeMin, volumeMax);
        musicVolumeText.text = defaultMusicVolumeText;
        musicVolumeSlider.value = GlobalSceneManager.MusicVolume;
        musicVolumeInput.text = "" + GlobalSceneManager.MusicVolume;
        
        // Dialogue Volume
        dialogueVolumeSlider.minValue = volumeMin;
        dialogueVolumeSlider.maxValue = volumeMax;
        defaultDialogueVolumeText = string.Format("Set volume value from {0} to {1}", volumeMin, 
            volumeMax);
        defaultDialogueVolumeErrorText = string.Format("Value not in range of {0}-{1}", volumeMin, volumeMax);
        dialogueVolumeText.text = defaultDialogueVolumeText;
        dialogueVolumeSlider.value = GlobalSceneManager.DialogueVolume;
        dialogueVolumeInput.text = "" + GlobalSceneManager.DialogueVolume;
    }
    
    // Slider functions to call when value changes
    private void SetMusicVolume(float value)
    {
        GlobalSceneManager.MusicVolume = value * GlobalSceneManager.GunVolumeMult;
        musicVolumeInput.text = value.ToString("0.00");  
        GlobalSceneManager.manager.SetMusicVolume();
    }

    private void SetGunVolume(float value)
    {
        GlobalSceneManager.GunVolume = value * GlobalSceneManager.GunVolumeMult;
        gunVolumeInput.text = value.ToString("0.00");
    }

    private void SetEnemyVolume(float value)
    {
        GlobalSceneManager.EnemyVolume = value * GlobalSceneManager.EnemyVolumeMult;
        enemyVolumeInput.text = value.ToString("0.00");
    }

    private void SetDialogueVolume(float value)
    {
        GlobalSceneManager.DialogueVolume = value * GlobalSceneManager.DialogueVolumeMult;
        dialogueVolumeInput.text = value.ToString("0.00");
    }

    private void SetSensitivity(float value)
    {
        GlobalSceneManager.SelectedSensitivity = value;
        sensitivityInput.text = value.ToString("0.00");
    }
    
    private void SetHealing(float value)
    {
        GlobalSceneManager.Healing = value;
        healingInput.text = value.ToString("0.00");
    }

    private void SetGunVolumeInput(string input)
    {
        GlobalSceneManager.GunVolume = AdjustSettingSlider(input, gunVolumeText, GlobalSceneManager.GunVolume,
            defaultGunVolumeText, defaultGunVolumeErrorText, volumeMin, volumeMax, gunVolumeSlider, gunVolumeInput);
    }

    private void SetEnemyVolumeInput(string input)
    {
        GlobalSceneManager.EnemyVolume = AdjustSettingSlider(input, enemyVolumeText, GlobalSceneManager.EnemyVolume,
            defaultEnemyVolumeText, defaultEnemyVolumeErrorText, volumeMin, volumeMax, enemyVolumeSlider, enemyVolumeInput);
    }

    private void SetMusicVolumeInput(string input)
    {
        GlobalSceneManager.MusicVolume = AdjustSettingSlider(input, musicVolumeText, GlobalSceneManager.MusicVolume,
            defaultMusicVolumeText, defaultMusicVolumeErrorText, volumeMin, volumeMax, musicVolumeSlider, musicVolumeInput);
        GlobalSceneManager.manager.SetMusicVolume();
    }

    private void SetDialogueVolumeInput(string input)
    {
        GlobalSceneManager.DialogueVolume = AdjustSettingSlider(input, dialogueVolumeText,
            GlobalSceneManager.DialogueVolume, defaultDialogueVolumeText, 
            defaultDialogueVolumeErrorText, volumeMin, volumeMax, dialogueVolumeSlider, dialogueVolumeInput);
    }
    
    private void SetSensitivityInput(string input)
    {
        /*// First try get the value
        sensitivityText.text = defaultSensitivityText;
        float value;
        try
        {
            value = float.Parse(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            sensitivityInput.text = ""+GlobalSceneManager.SelectedSensitivity;
            sensitivityText.text = $"{input} is not a valid sensitivity value";
            throw;
        }

        if (value < sensitivityMin | value > sensitivityMax)
        {
            sensitivityInput.text = ""+GlobalSceneManager.SelectedSensitivity;
            sensitivityText.text = $"{input} is not a valid sensitivity value";
            return;
        }
        
        // Then set the value
        slider.value = value;
        GlobalSceneManager.SelectedSensitivity = value;*/
        GlobalSceneManager.SelectedSensitivity = AdjustSettingSlider(input, sensitivityText, 
            GlobalSceneManager.SelectedSensitivity, defaultSensitivityText, 
            defaultSensitivityErrorText, sensitivityMin, sensitivityMax, sensitivitySlider, sensitivityInput);
    }

    private void SetHealingInput(string input)
    {
        GlobalSceneManager.Healing = AdjustSettingSlider(input, healingText, GlobalSceneManager.Healing,
            defaultHealingText, defaultHealingErrorText, healingMin, healingMax, healingSlider, healingInput);
    }

    /*private void SetDifficulty(float value)
    {
        GlobalSceneManager.DifficultySetting = (int)value;
        difficultyText.text = difficultyNames[(int)value];
    }*/
    
    private float AdjustSettingSlider(string input, TextMeshProUGUI textUI, float defaultValue,
        string defaultText, string errorText, float minVal, float maxVal, Slider slider, TMP_InputField inputField)
    {
        // First try get the value
        textUI.text = defaultText;
        float value;
        try
        {
            value = float.Parse(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            textUI.text = ""+defaultValue;
            textUI.text = errorText;
            throw;
        }

        if (value < minVal | maxVal < value)
        {
            inputField.text = ""+defaultValue;
            // textUI.text = errorText;
            textUI.text = $"{errorText} with value {value}";
            return defaultValue;
        }
        
        // Then set the value
        slider.value = value;
        defaultValue = value;
        return defaultValue;
    }
}
