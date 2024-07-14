using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    private Vector3 leftInitialPosition;
    private Vector3 rightInitialPosition;
    public Vector3 openOffset;
    public float timeToOpen;
    public float timeToClose;
    public float openPercent = 0;
    public bool opening = false;
    
    private void Start()
    {
        leftInitialPosition = leftDoor.transform.position;
        rightInitialPosition = rightDoor.transform.position;
        opening = false;
    }

    public void Update()
    {
        if (opening)
        {
            if (openPercent < 1.0f)
            {
                openPercent += Time.deltaTime / timeToOpen;
                
            }

            if (openPercent > 1.0f)
            {
                openPercent = 1.0f;
            }
        }
        else
        {
            
            if (openPercent > 0)
            {
                openPercent -= Time.deltaTime / timeToClose;
                
            }
            if (openPercent < 0)
            {
                openPercent = 0;
            }
        }
        leftDoor.transform.position = leftInitialPosition + openPercent * openOffset;
        rightDoor.transform.position = rightInitialPosition + -openPercent * openOffset;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            opening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player") || LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            opening = false;
        }
    }
}
