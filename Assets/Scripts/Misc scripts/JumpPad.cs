using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    /*    [Range(100, 10000)]

        public float launchHeight;

        private void OnCollisionEnter(Collision collision)
        {
            GameObject lunacher = collision.gameObject;
            Rigidbody rb = lunacher.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * launchHeight);
        }
    */

    [SerializeField]
    private float jumpForce = 10.0f; // You can change this value in the Unity Editor

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with the jump pad is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate the desired jump direction (upward)
            Vector3 jumpDirection = transform.up * jumpForce;

            // Apply the jump direction as an instant velocity (no need for a Rigidbody)
            collision.gameObject.GetComponent<CharacterController>().Move(jumpDirection);

            // You may also add some additional effects or animations here if desired
        }
    }

}
