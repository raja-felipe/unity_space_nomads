using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunSelectionScript : MonoBehaviour
{
    private const int MinGunIndex = 0;
    private int maxPrimaryIndex;
    private int maxSecondaryIndex;
    private int currPrimaryIndex = 0;
    private int currSecondaryIndex = 1;
    [SerializeField] private GunData[] primaryList;
    private GunData currPrimary;
    [SerializeField] private GunData[] secondaryList;
    private GunData currSecondary;
    [SerializeField] private Image primarySprite;
    [SerializeField] private Image secondarySprite;
    [SerializeField] private TextMeshProUGUI primaryGunName;
    [SerializeField] private TextMeshProUGUI secondaryGunName;
    [SerializeField] private TextMeshProUGUI primaryAmmo;
    [SerializeField] private TextMeshProUGUI secondaryAmmo;
    [SerializeField] private TextMeshProUGUI primaryDesc;
    [SerializeField] private TextMeshProUGUI secondaryDesc;
    /*public static GunData[] LocalSelectedGuns = { null, null };
    public static readonly int PrimaryIndex = 0;
    public static readonly int SecondaryIndex = 1;*/
    
    // Start is called before the first frame update
    void Start()
    {
        maxPrimaryIndex = primaryList.Length - 1;
        maxSecondaryIndex = secondaryList.Length - 1;
        currPrimary = primaryList[MinGunIndex];
        currSecondary = secondaryList[MinGunIndex+1];
        GlobalSceneManager.SelectedGuns =
            GlobalSceneManager.SelectedGuns != null ? GlobalSceneManager.SelectedGuns : new GunData[2];
        GlobalSceneManager.SelectedGuns[GlobalSceneManager.PrimaryGunIndex] = currPrimary;
        GlobalSceneManager.SelectedGuns[GlobalSceneManager.SecondaryGunIndex] = currSecondary;
        UpdateGunValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGunValues();
    }
    
    // Internal Functions
    private void UpdateGunValues()
    {
        SetGunSprites();
        SetAmmoCounts();
        SetGunDescriptions();
        SetGunNames();
        // ShowSelectGuns();
        SetGuns();
        // ShowConfirmGuns();
    }
    
    private void SetGunSprites()
    {
        primarySprite.sprite = currPrimary.gunSprite;
        secondarySprite.sprite = currSecondary.gunSprite;
    }

    private void SetGunDescriptions()
    {
        primaryDesc.text = currPrimary.gunDescription;
        secondaryDesc.text = currSecondary.gunDescription;
    }

    private void SetAmmoCounts()
    {
        primaryAmmo.text = string.Format("Clip: {0:00}, Ammo: {1:00}", currPrimary.clipSize, currPrimary.totalAmmo);
        secondaryAmmo.text = string.Format("Clip: {0:00}, Ammo: {1:00}", currSecondary.clipSize, currSecondary.totalAmmo);
    }

    private void SetGunNames()
    {
        primaryGunName.text = currPrimary.gunName;
        secondaryGunName.text = currSecondary.gunName;
    }

    /*private void ShowConfirmGuns()
    {
        bool primaryNotNull = GlobalSceneManager.SelectedGuns[GlobalSceneManager.PrimaryGunIndex] != null;
        bool secondaryNotNull = GlobalSceneManager.SelectedGuns[GlobalSceneManager.SecondaryGunIndex] != null;
        if (primaryNotNull && secondaryNotNull)
        {
            // Debug.Log("BOTH ITEMS READY");
            startMissionTMP.color = Color.black;
            startMissionButton.GetComponent<Image>().color = Color.white;
            cancelButtonTMP.color = Color.black;
            cancelButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            // Debug.Log("NOT READY");
            startMissionTMP.color = Color.clear;
            startMissionButton.GetComponent<Image>().color = Color.clear;
            cancelButtonTMP.color = Color.clear;
            cancelButton.GetComponent<Image>().color = Color.clear;
        }
    }*/

    // Functions for buttons
    public void SetGuns()
    {
        SetPrimaryGun();
        SetSecondaryGun();
    }

    public void SetPrimaryGun()
    {
        GlobalSceneManager.SelectedGuns[GlobalSceneManager.PrimaryGunIndex] = currPrimary;
    }

    public void SetSecondaryGun()
    {
        GlobalSceneManager.SelectedGuns[GlobalSceneManager.SecondaryGunIndex] = currSecondary;
    }
    
    /*public void UnsetSecondaryGun()
    {
        GlobalSceneManager.SelectedGuns[GlobalSceneManager.SecondaryGunIndex] = null;
        primarySelected = false;
    }*/

    public void GoToPrimaryGun(int shiftAmount)
    {
        currPrimaryIndex += shiftAmount;
        
        if (currPrimaryIndex > maxPrimaryIndex)
        {
            currPrimary = primaryList[MinGunIndex];
            currPrimaryIndex = MinGunIndex;
        }
        
        else if (currPrimaryIndex < MinGunIndex)
        {
            currPrimary = primaryList[maxPrimaryIndex];
            currPrimaryIndex = maxPrimaryIndex;
        }

        else
        {
            currPrimary = primaryList[currPrimaryIndex];
        }
        
        // One last check, have to increment more if the guns are the same
        if (currPrimary.gunName == currSecondary.gunName)
        {
            GoToPrimaryGun(shiftAmount);
        }
    }

    public void GoToSecondaryGun(int shiftAmount)
    {
        currSecondaryIndex += shiftAmount;
        
        if (currSecondaryIndex > maxSecondaryIndex)
        {
            currSecondary = secondaryList[MinGunIndex];
            currSecondaryIndex = MinGunIndex;
        }
        
        else if (currSecondaryIndex < MinGunIndex)
        {
            currSecondary = secondaryList[maxSecondaryIndex];
            currSecondaryIndex = maxSecondaryIndex;
        }

        else
        {
            currSecondary = secondaryList[currSecondaryIndex];
        }
        
        if (currPrimary.gunName == currSecondary.gunName)
        {
            GoToSecondaryGun(shiftAmount);
        }
    }
    
    /*public void GoToNextPrimaryGun()
    {
        if (primarySelected)
        {
            return;
        }
        
        if (currPrimaryIndex == maxPrimaryIndex)
        {
            currPrimary = primaryList[MinGunIndex];
            currPrimaryIndex = MinGunIndex;
        }
        else
        {
            currPrimaryIndex++;
            currPrimary = primaryList[currPrimaryIndex];
        }
    }

    public void GoToPrevPrimaryGun()
    {
        if (primarySelected)
        {
            return;
        }
        
        if (currPrimaryIndex == MinGunIndex)
        {
            currPrimary = primaryList[maxPrimaryIndex];
            currPrimaryIndex = maxPrimaryIndex;
        }
        else
        {
            currPrimaryIndex--;
            currPrimary = primaryList[currPrimaryIndex];   
        }
    }
    
    public void GoToNextSecondaryGun()
    {
        if (secondarySelected)
        {
            return;
        }
        
        if (currSecondaryIndex == maxSecondaryIndex)
        {
            currSecondary = secondaryList[MinGunIndex];
            currSecondaryIndex = MinGunIndex;
        }
        else
        {
            currSecondaryIndex++;
            currSecondary = secondaryList[currSecondaryIndex];   
        }
    }

    public void GoToPrevSecondaryGun()
    {
        if (secondarySelected)
        {
            return;
        }
        
        if (currSecondaryIndex == MinGunIndex)
        {
            currSecondary = secondaryList[maxSecondaryIndex];
            currSecondaryIndex = maxSecondaryIndex;
        }
        else
        {
            currSecondaryIndex--;
            currSecondary = secondaryList[currSecondaryIndex];
        }
    }*/
    
    // Use this to set the final gun values
    /*public void SetGuns()
    {
        GlobalSceneManager.SelectedGuns[PrimaryIndex] =
            LocalSelectedGuns[PrimaryIndex];
        GlobalSceneManager.SelectedGuns[SecondaryIndex] =
            LocalSelectedGuns[SecondaryIndex];
    }*/
}