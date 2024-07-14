using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairDisplay : MonoBehaviour
{
    private Animator crosshairAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        crosshairAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) | Input.GetMouseButtonDown(1))
        {
            PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        if (PlayerGunScript.currentGunScript.gunCooldown <= 0f)
        {
            crosshairAnimator.SetTrigger("Shoot");
        }
    }
}
