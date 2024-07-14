using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    public Material controledMaterial;
    public float switchSpeed = 1.0f;

    private Color[] colorsForSwitch = { new Color(0.5f, 0, 0.5f), Color.green, new Color(1, 0.5f, 0.5f) };

    private int currentColor = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(changeEmissiveColorOnMaterial());
    }

    IEnumerator changeEmissiveColorOnMaterial()
    {
        while (true)
        {
            Color currentEmissiveColor = controledMaterial.GetColor("_EmissionColor");
            Color targetColor = colorsForSwitch[currentColor];

            while (Vector3.Distance(ColorToVector3(currentEmissiveColor), ColorToVector3(targetColor)) > 0.01f)
            {
                currentEmissiveColor = Color.Lerp(currentEmissiveColor, targetColor, switchSpeed * Time.deltaTime);
                controledMaterial.SetColor("_EmissionColor", currentEmissiveColor * 3.0f);
                yield return null;
            }

            currentColor = (currentColor + 1) % colorsForSwitch.Length;
        }
    }

    Vector3 ColorToVector3(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }
}
