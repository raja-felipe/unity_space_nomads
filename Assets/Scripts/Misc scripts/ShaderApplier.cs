using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
    

public class ShaderApplier : MonoBehaviour
{
    // Start is called before the first frame update
    public Material thisMaterial;
    public Color currentColor;
    public Texture currentTexture;
    public float k = 0.5f;
    public float powerFactor = 1f;
    public float scalingFactor = 1f;
    public void Start()
    {
        thisMaterial = this.GetComponent<SkinnedMeshRenderer>().material;
        
    }

    public int colourInt = 0;
    public void Update()
    {
        thisMaterial.SetTexture("_MainTex",currentTexture);
        thisMaterial.SetFloat("k",k);
        //currentColor = new Color(((float) colourInt / 65536)/256, ((float)(colourInt % (65536)) / 256)/256,  ((float)colourInt % 256)/256);
        thisMaterial.SetColor( "_Color" , currentColor);
        thisMaterial.SetFloat("_PowerFactor",powerFactor);
        thisMaterial.SetFloat("_ScalingFactor",scalingFactor);
        //colourInt += 10;
    }
}

