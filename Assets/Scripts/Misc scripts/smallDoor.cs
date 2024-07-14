using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class smallDoor : MonoBehaviour
{
    public Animation anim;
    private bool isDoorOpen = false;

    public string openAnimation = "sciDoorOpen";
    public string closeAnimation = "sciDoorClose";

    public void Start()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || LayerMask.LayerToName(other.gameObject.layer) == "Enemy") && !isDoorOpen)
        {
            anim.Play(openAnimation);
            StartCoroutine(WaitForOpenAnimation());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player") || LayerMask.LayerToName(other.gameObject.layer) == "Enemy") && isDoorOpen )
        {
            StartCoroutine(WaitAndPlayCloseAnimation());
        }
    }

    private System.Collections.IEnumerator WaitForOpenAnimation()
    {
        while (anim.isPlaying)
        {
            yield return null;
        }

        isDoorOpen = true;  // Set the door state to open after the open animation finishes
    }

    private System.Collections.IEnumerator WaitAndPlayCloseAnimation()
    {
        while (anim.isPlaying)
        {
            yield return null;  // Wait for the open animation to finish if it's still playing
        }

        anim.Play(closeAnimation);  // Play the close animation
        isDoorOpen = false;  // Set the door state to closed after the close animation starts
    }
}
