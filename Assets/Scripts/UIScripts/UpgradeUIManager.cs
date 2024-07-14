using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public Image[] upgradeImages;
    public TextMeshProUGUI[] upgradeTexts;
    public TextMeshProUGUI[] upgradeDescriptions;
    public Button[] upgradeButtons;
    [SerializeField] private int numberOfUpgrades = 3;
    // Store the guns and gun upgrades so that you can undo the changes
    // when the game finishes
    private Dictionary<GunUpgrade, bool> gunUpgradesApplied;
    private List<GunUpgrade> allGunUpgrades;
    private List<int> upgradeToGun;
    private List<GunUpgrade> selectedGunUpgrades;
    private List<int> selectedIndices;
    private int totalNumberOfGuns;
    private bool selected=false;
    // Start is called before the first frame update
    void Start()
    {
        totalNumberOfGuns = PlayerGunScript.currentGunScript.equippedGuns.Length - 1;
        allGunUpgrades = new List<GunUpgrade>();
        upgradeToGun = new List<int>();
        InitializeGunUpgrades();
    }

    void OnDisable()
    {
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (upgradeCanvas.activeSelf && !selected)
        {
            selectedGunUpgrades = new List<GunUpgrade>();
            selectedIndices = new List<int>();
            InitializeUIElements();
            RandomlySelectGunUpgrades();
            selected = true;
        }
    }
    
    // Initialize all possible gun upgrades
    private void InitializeGunUpgrades()
    {
        for (int i = 0; i < totalNumberOfGuns; i++)
        {
            for (int j = 0; j < PlayerGunScript.currentGunScript.equippedGuns[i].data.possibleUpgrades.Length; j++)
            {
                GunUpgrade currUpgrade = PlayerGunScript.currentGunScript.equippedGuns[i].data.possibleUpgrades[j];
                allGunUpgrades.Add(currUpgrade);
                upgradeToGun.Add(i);
            }
        }
    }

    private void InitializeUIElements()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeImages[i].color = Color.clear;
            upgradeTexts[i].color = Color.clear;
            upgradeDescriptions[i].color = Color.clear;
            upgradeButtons[i].GetComponent<Image>().color = Color.clear;
        }
    }
    
    // Functions to randomize the upgrades
    private void RandomlySelectGunUpgrades()
    {
        int numIndices = Mathf.Min(numberOfUpgrades, allGunUpgrades.Count);
        selectedIndices = GenerateRandomIndices(numIndices);
        
        // After getting the numbers, we can now get the necessary values
        for (int i = 0; i < numIndices; i++)
        {
            /*Debug.Log("NUM INDEX: "+i);
            Debug.Log("UPGRADE INDEX: "+selectedIndices[i]);*/
            UpdateUIElements(selectedIndices[i], upgradeToGun[selectedIndices[i]], i);
            selectedGunUpgrades.Add(allGunUpgrades[selectedIndices[i]]);
        }
    }
    
    // Use this to update the UI elements
    private void UpdateUIElements(int upgradeIndex, int gunIndex, int imageIndex)
    {
        // Set proper colors
        upgradeImages[imageIndex].color = Color.white;
        upgradeTexts[imageIndex].color = Color.black;
        upgradeDescriptions[imageIndex].color = Color.white;
        upgradeButtons[imageIndex].GetComponent<Image>().color = Color.white;
        
        // Set the data to display on UI
        GunData currGunData = PlayerGunScript.currentGunScript.equippedGuns[gunIndex].data;
        GunUpgrade upgrade = allGunUpgrades[upgradeIndex];
        upgradeImages[imageIndex].sprite = currGunData.gunSprite;
        upgradeTexts[imageIndex].text = upgrade.name;
        upgradeDescriptions[imageIndex].text = upgrade.description;
    }

    public void ApplyUpgradeToGun(int index)
    {
        if (allGunUpgrades.Count < index) return;
        // First extract the gundata and the gunUpgrade
        PlayerGunScript.currentGunScript.equippedGuns[upgradeToGun[selectedIndices[index]]].data.ApplyUpgrade(allGunUpgrades[selectedIndices[index]]);
        // After applying said upgrade, we need to remove it from our total list of ugprades
        allGunUpgrades.RemoveAt(selectedIndices[index]);
        upgradeToGun.RemoveAt(selectedIndices[index]);
    }
    
    private List<int> GenerateRandomIndices(int numIndices)
    {
        HashSet<int> numSet = new HashSet<int>();
        while (numSet.Count < numIndices)
        {
            numSet.Add(Random.Range(0, allGunUpgrades.Count));
        }

        return numSet.ToList();
    }
}
