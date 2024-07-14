using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControlScript : MonoBehaviour
{
    public static PlayerControlScript currentPlayer;
    /// <summary>
    /// This script is used for controlling the player. Takes inputs and commands other scripts to do actions.
    /// Also does movement.
    ///
    /// </summary>
    public CharacterController controller;
    private const float BaseSensitivity = 2f;
    public float mouseSensitivity = 10f;
    public float interactRange = 2f;
    public float speed = 5f;
    public float jumpHeight = 3f;
    public float jumpDelay = 0.1f;
    public float jumpTimer = 0.1f;
    public float airControlMultiplier = 0.5f;
    public float sprintMultiplier = 1.2f;
    public Vector3 velocity;
    public bool isGrounded = false;
    public float gravityMultiplier;
    public float yCameraRotation = 0f;
    public Camera playerCamera;
    public PlayerGunScript thisGunScript;
    public GameObject currentInteractableAtCrosshair;
    public bool canInteract = false;
    public float dampenMouse = 0.3f;
    [SerializeField] private GameUIManager uiManager;

    private void Awake()
    {
        currentPlayer = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Ensure there are no constraints in the rigidbody
        /*if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            mouseSensitivity = 1f;
        }*/
        Debug.Log($"SELECTED SENSITIVITY: {BaseSensitivity * GlobalSceneManager.SelectedSensitivity}");
    }

    void Update()
    {
        // Get inputs:
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // Always update the mouseSensitivity
        mouseSensitivity = BaseSensitivity * GlobalSceneManager.SelectedSensitivity;
        
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            mouseX = DampenedMovement (mouseX);
            mouseY = DampenedMovement (mouseY);
        }

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;
        float jumping = Input.GetAxis("Jump");
        float fire1 = Input.GetAxis("Fire1");
        float cycleWeapon = Input.GetAxis("Mouse ScrollWheel");
        float altFire = Input.GetAxis("Fire2"); 
        //the below inputs are fixed, can't be changed.
        
        bool sprinting = Input.GetKey(GlobalSceneManager.inputLabelList["Sprint"]);
        bool interact = Input.GetKeyDown(GlobalSceneManager.inputLabelList["Interact"]);
        bool reloading = Input.GetKey(GlobalSceneManager.inputLabelList["Reload"]);
        bool equipPrimary = Input.GetKeyDown(KeyCode.Alpha1);
        bool equipSecondary = Input.GetKeyDown(KeyCode.Alpha2);
        bool equipTool = Input.GetKeyDown(KeyCode.Alpha3);
        bool useGrenade = Input.GetKeyDown(KeyCode.Alpha4);
        
        Vector3 gravity = Physics.gravity * gravityMultiplier;
        Vector3 move = transform.right * x + transform.forward * z;
        
        jumpTimer += Time.deltaTime;
        //if can jump, do the jumping.
        if (jumping >= 1 && isGrounded && jumpTimer > jumpDelay)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity.y);
            isGrounded = false;
            jumpTimer = 0;
        }
        if (sprinting)
        {
            if (isGrounded)
            {
                move *= sprintMultiplier;
            }
            // shoot stuff
        }

        // Should only do these actions if the game is in play state
        if (thisGunScript != null && !uiManager.isPaused && !uiManager.inUpgrades && !uiManager.inControls)
        {
            // thisGunScript.beginReload();
            if (fire1 >= 1)
            {
                thisGunScript.primaryFire(this);
                // shoot stuff
            }
            // thisGunScript.beginReload();
            if (altFire >= 1)
            {
                thisGunScript.altFire(this);
                // alt shoot stuff
            }
            if (reloading)
            {
                thisGunScript.beginReload();
                //reload gun 
            }
            // Auto reload should only apply to all guns except tool gun and hammer
            if (!reloading && !thisGunScript.currentGun.data.gunName.Equals("ToolGun") && 
                !thisGunScript.currentGun.data.gunName.Equals("Hammer Time"))
            {
                thisGunScript.beginReload();
                //reload gun 
            }
            
            if (cycleWeapon > 0f) // if scroll wheel is moved forward, cycleWeapon is positive.
            {
                thisGunScript.cycleWeapon(1);
            }
            if (cycleWeapon < 0f)
            {
                thisGunScript.cycleWeapon(-1);
            }

            if (equipPrimary)
            { 
                thisGunScript.selectWeapon(0);
            }
            
            if (equipSecondary)
            {
                thisGunScript.selectWeapon(1);
                
            }
            
            if (equipTool)
            {
                thisGunScript.selectWeapon(2);
            }

            if (useGrenade)
            {
                thisGunScript.useGrenade();
            }
        }

        RaycastHit hit;
        canInteract = false;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward),
                out hit, interactRange))
        {
            Transform hitTransform = hit.transform.root;
            if (hitTransform.CompareTag("Interactable"))
            {
                currentInteractableAtCrosshair = hitTransform.gameObject;
                canInteract = true;
            }
        }
        if (interact)
        {
            if (canInteract)
            {
                // currentInteractableAtCrosshair.SendMessage("Interact");
                currentInteractableAtCrosshair.gameObject.GetComponent<OnInteractDisplay>().TriggerInteraction();
            }
            // interact stuff
            
        }
        // If we are touching ground, our velocity cannot be downwards, and if we aren't, then gravity occurs.
        if (!isGrounded)
        {
            move *= airControlMultiplier;
            velocity += gravity * Time.deltaTime;
        }
        else
        {
            if (velocity.y < 0)
            {
                velocity.y = 0;
            }
        }
        
        // camera rotation and stuff.
        
        // Need to apply freezing camera transformation here
        if (uiManager.isPaused | uiManager.lostGame | uiManager.inUpgrades)
        {
            //playerCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else
        {
            yCameraRotation -= mouseY;
            yCameraRotation = Mathf.Clamp(yCameraRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(yCameraRotation, 0f, 0f);
            controller.transform.Rotate(Vector3.up * mouseX);
        }
        // apply controls and velocity all at once.
        controller.Move(velocity * Time.deltaTime + speed * Time.deltaTime * move);
    }
    public float DampenedMovement (float value) {
 
        if (Mathf.Abs (value) > 1f) {
            // best value for dampenMouse is 0.5 but better make it user-adjustable
            return Mathf.Lerp (value, Mathf.Sign (value), dampenMouse);
        }
        return value;
    }
}
