using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f; // Time before the platform falls
    public float resetDelay = 3f; // Time before the platform resets
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    private void Start()
    {
        // Save the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ensure it doesn't fall initially
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has landed on the platform
        if (collision.collider.CompareTag("Player"))
        {
            Invoke(nameof(Fall), fallDelay);
        }
    }

    private void Fall()
    {
        rb.isKinematic = false; // Allow the platform to fall
        Invoke(nameof(ResetPlatform), resetDelay);
    }

    private void ResetPlatform()
    {
        // Reset position and stop any movement
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
