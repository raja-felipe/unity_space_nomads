using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InteractableData data;

    public void Interact()
    {
        data.onInteract(this.gameObject);
    }
}
