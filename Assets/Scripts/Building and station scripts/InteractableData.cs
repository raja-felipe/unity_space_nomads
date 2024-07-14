using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableData : ScriptableObject
{
    public const string Tag = "Interactable";
    public abstract void onInteract(GameObject interactableObject);
}
