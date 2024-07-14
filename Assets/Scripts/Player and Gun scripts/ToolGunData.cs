using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ToolGunData : GunData
{
    public int currentBuildingSelected = 0;

    public buildableObjectScript[] BuildableObjects
    {
        get { return gameManagerScript.Buildings; }
    }

    public buildableObjectScript currentBuildable
    {
        get
        {
            return BuildableObjects[currentBuildingSelected % BuildableObjects.Length];
        }
    }
    public float buildingRotation;
    public Vector3 currentLookPosition;
    public Vector3 currentLookRotation;
    public bool lookPositionIsWellDefined;
    public float rotateDelay = 0.25f;
    public bool canRotateBuilding = true;
    public override void fire(Camera playerCamera, PlayerGunScript gunScript)
    {
        updateLookPosition(playerCamera,gunScript);
        gunScript.currentGun.currentClip = 2;
        if (PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources < currentBuildable.cost)
        {
            return;
        }
        Vector3 shootDirection = currentLookRotation;
        if (lookPositionIsWellDefined)
        {
            Instantiate(currentBuildable, currentLookPosition + currentBuildable.BuildingOffset,
            Quaternion.Euler(0,shootDirection.y + buildingRotation + currentBuildable.RotationAdd,0));
            //rebuild mesh here?
            PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources -= currentBuildable.cost;
            HudUiManager.HudManager.ShowGooChange(currentBuildable.cost, false);
        }
    }
    /// <summary>
    /// switches weapon on alt fire.
    /// </summary>
    public override void altFire(Camera playerCamera)
    {
        if (canSwitchBuilding)
        {
            gameManagerScript.manager.buildingHolograms[currentBuildingSelected].SetActive(false);
            currentBuildingSelected += 1;
            currentBuildingSelected %= BuildableObjects.Length;
            gameManagerScript.manager.buildingHolograms[currentBuildingSelected].SetActive(true);
            gameManagerScript.manager.StartCoroutine(this.switchBuildingTimerRoutine());
        }
    }

    public override void onReload(PlayerGunScript gunScript)
    {
        if (canRotateBuilding)
        {
            gunScript.currentGun.currentClip = 1;
            buildingRotation += 45;
            buildingRotation %= 360;
            gunScript.StartCoroutine(rotationTimerRoutine());
        }
    }
    
    public void updateLookPosition(Camera playerCamera, PlayerGunScript gunScript)
    {
        Transform cameraTransform = playerCamera.transform;
        Vector3 shootDirection = cameraTransform.forward;
        shootDirection.Normalize();
        currentLookRotation = shootDirection;
        Vector3 shootFromPos = cameraTransform.position + cameraTransform.rotation * muzzlePosition;
        RaycastHit hit;
        if (Physics.Raycast(shootFromPos, shootDirection, out hit, Mathf.Infinity,gameManagerScript.manager.buildableLayers))
        {
            currentLookPosition = hit.point;
            lookPositionIsWellDefined = true;
            
            gameManagerScript.manager.buildingHolograms[currentBuildingSelected].SetActive(true);
        }
        else
        {
            lookPositionIsWellDefined = false;
            
            gameManagerScript.manager.buildingHolograms[currentBuildingSelected].SetActive(false);
        }
    }
    public IEnumerator rotationTimerRoutine()
    {
        canRotateBuilding = false;
        yield return new WaitForSeconds(rotateDelay);
        canRotateBuilding = true;
        
    }
    public bool canSwitchBuilding = true;
    public float buildingSwitchDelay = 0.25f;
    public IEnumerator switchBuildingTimerRoutine()
    {
        canSwitchBuilding = false;
        yield return new WaitForSeconds(buildingSwitchDelay);
        canSwitchBuilding = true;
    }
    public override void update(Camera playerCamera, PlayerGunScript gunScript)
    {
        updateLookPosition(playerCamera,gunScript);
        // move current hologram
        GameObject currentHolo = gameManagerScript.manager.buildingHolograms[currentBuildingSelected];
        currentHolo.transform.position = currentLookPosition + currentBuildable.BuildingOffset;
        currentHolo.transform.rotation = Quaternion.Euler(0,currentLookRotation.y + buildingRotation + currentBuildable.RotationAdd,0);
    }
}
