using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float xSpeed = 20f;
    public float ySpeed = 20f;
    public float zSpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        float rotatingX = xSpeed * Time.deltaTime;
        float rotatingY = ySpeed * Time.deltaTime;
        float rotatingZ = zSpeed * Time.deltaTime;

        transform.Rotate(rotatingX, rotatingY, rotatingZ);
    }
}
