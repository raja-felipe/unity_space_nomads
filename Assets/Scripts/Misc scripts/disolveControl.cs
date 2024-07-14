using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disolveControl : MonoBehaviour
{
    public Material orginMaterial;
    private Material targetingMaterial;

    public float speed = 0.3f, max;

    private float currentY;

    // Update is called once per frame
    private void Awake()
    {
        targetingMaterial = new Material(orginMaterial);

        GetComponent<Renderer>().material = targetingMaterial;
    }

    void Update()
    {
        if (currentY < max)
        {
            targetingMaterial.SetFloat("_DisolveY", currentY);

            currentY += Time.deltaTime * speed;
        }
    }
}
