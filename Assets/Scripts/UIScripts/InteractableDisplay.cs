using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactableText;
    [SerializeField] private Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        interactableText.enabled = false;
    }

    // Update is called once per frame
    void Update()
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
    }
}
