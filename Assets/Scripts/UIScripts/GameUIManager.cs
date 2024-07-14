using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public bool isPaused = false;
    public bool lostGame = false;
    public bool inControls = false;
    public bool inUpgrades = false;
    [SerializeField] private GameObject hudCanvas;
    // Other items that need to be disabled in the canvas
    [SerializeField] private GameObject hudSuperiorVisor;
    [SerializeField] private GameObject hudInferiorVisor;
    [SerializeField] private GameObject hudRenderCamera;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private GameObject controlCanvas;
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject playerHitCanvas;
    [SerializeField] private GameObject playerHitMarker;
    [SerializeField] private float playerHitMarkerRadius;
    [SerializeField] private float playerHitMarkerDuration;
    
    
    [SerializeField] private gameManagerScript gameManager;
    [SerializeField] private TimerDisplay timerDisplay;
    
    void Start()
    {
        GetReferences();
        SetActiveHud(true);
    }

    private void Update()
    {
        // Check if the player died
        if (PlayerHealthScript.CurrentPlayerHealthScript.health <= 0)
        {
            SetActiveDeath(true);
        }
        
        // UNCOMMENT FOR LATER, DOES NOT NEED TO BE HERE
        // USED FOR TESTING THE GUN UPGRADES
        /*else if (Input.GetKeyDown(KeyCode.Y))
        {
            SetActiveUpgrade(true);
        }*/
        
        // Check first if the player paused
        else if (Input.GetKeyDown(KeyCode.Tab) && !isPaused && !inControls && !lostGame && !inUpgrades)
        {
            SetActivePause(true);
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && isPaused && !inControls && !lostGame && !inUpgrades)
        {
            SetActivePause(false);
        }
        // Each update, make sure we rerender the HUD texture
        // UpdateHudOnRender();
        // Canvas.ForceUpdateCanvases(); // Force update all the canvases
    }
    
    // Helper Function to Render the WHOLE Hud UI
    private void SetActiveHelmet(bool state)
    {
        hudCanvas.SetActive(state);
        hudInferiorVisor.SetActive(state);
        hudSuperiorVisor.SetActive(state);
        hudRenderCamera.SetActive(state);
    }

    // Activate UI canvases
    public void SetActiveHud(bool state)
    {
        SetActiveHelmet(state);
        deathCanvas.SetActive(!state);
        pauseCanvas.SetActive(!state);
        controlCanvas.SetActive(!state);
        upgradeCanvas.SetActive(!state);
    }

    private void SetActivePause(bool state)
    {
        pauseCanvas.SetActive(state);
        SetActiveHelmet(!state);
        
        // Check to freeze the camera
        /*if (state)
        {
            Debug.Log("FREEZING CAMERA");
            PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints = 
                RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        }

        else
        {
            Debug.Log("UNFREEZING CAMERA");
            PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.None;
        }*/

        Time.timeScale = state ? 0 : 1;

        if (state)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        isPaused = state;
    }

    private void SetActiveControl(bool state)
    {
        pauseCanvas.SetActive(!state);
        controlCanvas.SetActive(state);

        inControls = state;
    }

    private void SetActiveUpgrade(bool state)
    {
        /*Debug.Log(hudCanvas != null);
        Debug.Log(upgradeCanvas != null);
        Debug.Log(deathCanvas != null);
        Debug.Log(controlCanvas != null);
        Debug.Log(pauseCanvas != null);*/
        SetActiveHelmet(!state);
        upgradeCanvas.SetActive(state);
        
        Time.timeScale = state ? 0 : 1;

        if (state)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        inUpgrades = state;
    }

    public void SetActiveDeath(bool state)
    {
        deathCanvas.SetActive(state);
        SetActiveHelmet(!state);
        pauseCanvas.SetActive(!state);
        controlCanvas.SetActive(!state);
        Time.timeScale = 0;
        
        // Check to freeze the camera
        /*if (state)
        {
            Debug.Log("FREEZING CAMERA");
            Debug.Log(PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints);
            PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints = 
                RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        }

        else
        {
            Debug.Log("UNFREEZING CAMERA");
            PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.None;
        }*/

        lostGame = state;
        if (state)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void GetReferences()
    {

    }

    // On button functions
    public void Resume()
    {
        SetActivePause(false);
        // Make sure to unfreze the camera rotation
        /*PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.None;*/
    }

    public void Settings()
    {
        Debug.Log("SETTINGS MENU");
        // Make sure to freeze the camera here
        /*PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;*/
    }

    public void Controls()
    {
        SetActiveControl(true);
    }

    public void LeaveControls()
    {
        SetActiveControl(false);
    }

    public void GameOver(string newFailText = "YOU FAILED")
    {
        SetActiveDeath(true);
        Debug.Log("FREEZING CAMERA");
        /*PlayerControlScript.currentPlayer.playerCamera.GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;*/
        // Set the failure text
        GlobalSceneManager.FailText = newFailText;
    }
    
    public void GoToUpgrade()
    {
        SetActiveUpgrade(true);
    }

    public void SkipUpgrade()
    {
        SetActiveUpgrade(false);
    }

    public void GetUpgrade()
    {
        SetActiveUpgrade(false);
        // Do upgrade methods here
    }

    private bool CheckIfUpgrade()
    {
        float timeLeft = gameManager.getTimeAtLastPause();

        return (int)(timeLeft) == 10;
    }

    public void Quit()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
    //spawns a marker for when the player is hit, at the angle specified. 0 angle is directly to the left, 90 is up.
    public void spawnPlayerHitMarker(float angle)
    {
        if (playerHitMarker != null && playerHitCanvas != null)
        {
            GameObject newHitMarker = Instantiate(playerHitMarker,
                playerHitMarkerRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle),-Mathf.Cos(Mathf.Deg2Rad * angle), 0).normalized + playerHitCanvas.transform.position,
                Quaternion.Euler(0, 0, angle), playerHitCanvas.transform);
            StartCoroutine(hitMarkerRoutine(newHitMarker));
            Destroy(newHitMarker, playerHitMarkerDuration);
        }
    }

    IEnumerator hitMarkerRoutine(GameObject hitMarker)
    {
        float remainingTime = playerHitMarkerDuration;
        Renderer r = hitMarker.GetComponent<Renderer>();
        float initialAlpha = r.material.color.a;
        while (remainingTime > 0)
        {
            Color tColor = r.material.color;
            tColor.a = initialAlpha * remainingTime / playerHitMarkerDuration;
            r.material.color = tColor;
            yield return new WaitForEndOfFrame();
            remainingTime -= Time.deltaTime;
        }
        Destroy(hitMarker);
        yield return null;
        
    }
}
