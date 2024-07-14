using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerControlScript this_player;
    public LayerMask WalkableLayers;
    private void OnTriggerStay(Collider other)
    {
        
        if (isWalkable(other))
        {
            if (this_player != null)
            {
                this_player.isGrounded = true;
            }
            else
            {
                Debug.Log("BRO GET UR ATTACHEMENTS RIGHT");
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (isWalkable(other))
        {
            if (this_player != null)
            {
                this_player.isGrounded = false;
            }
            else
            {
                Debug.Log("BRO GET UR ATTACHEMENTS RIGHT");
            }
        }
    }

    public bool isWalkable(Collider other)
    {
        return !other.CompareTag("NotWalkable") &&
               (other.CompareTag("Walkable") || (WalkableLayers & 1 << other.gameObject.layer) != 0);
    }
}
