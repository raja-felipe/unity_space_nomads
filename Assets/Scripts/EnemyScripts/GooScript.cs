using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class GooScript : MonoBehaviour
{
    public int gooValue = 0;

    public float pickupRange = 0.5f;
    public float attractionRange = 4f;

    public float attractionSpeed = 10f;

    public AudioClip gooSound = null;

    public float gooVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToPlayer = PlayerControlScript.currentPlayer.transform.position - transform.position;
        float distanceToPlayer = (vectorToPlayer).magnitude;
        if (distanceToPlayer < pickupRange)
        {
            PlayerHealthScript.CurrentPlayerHealthScript.currentOwnedResources += gooValue;
            HudUiManager.HudManager.ShowGooChange(gooValue, true);
            if (gooSound != null)
            {
                GetComponent<AudioSource>().PlayOneShot(gooSound,gooVolume);
            }

            Destroy(this.gameObject);
        }
        if (distanceToPlayer < attractionRange)
        {
            transform.position += attractionSpeed * vectorToPlayer.normalized / Mathf.Pow(distanceToPlayer + attractionRange * 0.1f,2) * Time.deltaTime;
        }
    }
}
