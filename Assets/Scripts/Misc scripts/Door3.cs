using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door3 : MonoBehaviour
{
    public Animation anim;
    private bool isDoorOpen = false;
    private bool isAnimating = false;

    public string openAnimation = "Door3Open";
    public string closeAnimation = "Door3Close";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDoorOpen && !isAnimating)
        {
            isAnimating = true;
            anim.Play(openAnimation);
            StartCoroutine(AnimationFinishCheck());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isDoorOpen && !isAnimating)
        {
            isAnimating = true;
            anim.Play(closeAnimation);
            StartCoroutine(AnimationFinishCheck());
        }
    }

    private System.Collections.IEnumerator AnimationFinishCheck()
    {
        while (anim.isPlaying)
        {
            yield return null;
        }

        isAnimating = false;
        isDoorOpen = !isDoorOpen;
    }
}
