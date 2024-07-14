using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInteractDisplay : MonoBehaviour
{
    public const string Tag = "OnInteractDisplay";
    [SerializeField] private string displayString;
    public IsInteractable interactableObject;
    // Start is called before the first frame update
    void Start()
    {
        interactableObject = GetComponent<IsInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        displayString = interactableObject.WriteInteractableText();
    }

    public string GetDisplayString()
    {
        return displayString;
    }

    public void SetDisplayString(string replace)
    {
        displayString = replace;
    }

    public void TriggerInteraction()
    {
        interactableObject.TriggerInteraction();
    }
}
