using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if (playerRb)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, bounceForce, playerRb.velocity.z);
            }
        }
    }
}
