using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetInitializer : MonoBehaviour
{
    [SerializeField] private Material helmetMaterial;
    // Start is called before the first frame update
    void Start()
    {
        // Get the MeshFilter component of the 3D object
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            // Debug.Log("FLIPPING NORMALS OF MESH");
            // Get the mesh
            Mesh mesh = meshFilter.mesh;

            // Reverse each of the normals present
            for (int i = 0; i < mesh.normals.Length; i++)
            {
                mesh.normals[i] = -mesh.normals[i];
            }
        }
        
        // After flipping the normals within unity, now add the material
        // to the mesh
        Renderer rend = GetComponent<Renderer>();

        if (rend != null)
        {
            rend.material = helmetMaterial;
        }

        else
        {
            Debug.Log("ADD A MATERIAL TO HELMET MESH");
        }

    }
}
