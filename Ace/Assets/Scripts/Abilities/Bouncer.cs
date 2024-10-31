using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 15f; // The force with which the player is bounced
    public Vector3 bounceDirection = Vector3.up; // Default bounce direction, upwards

    // Tag to filter what objects can use the bounce pad, for example, "Player"
    public string bounceTargetTag = "Player";

    // You can toggle whether the bounce uses the pad's local rotation for more dynamic bounces
    public bool usePadRotation = true;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that hit the pad has the correct tag (e.g., "Player")
        if (other.CompareTag(bounceTargetTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Zero out current velocity to prevent stacking forces
                rb.velocity = Vector3.zero;

                // Calculate the bounce direction based on the pad's rotation if enabled
                Vector3 direction = bounceDirection;
                if (usePadRotation)
                {
                    direction = transform.TransformDirection(bounceDirection.normalized);
                }

                // Apply the bounce force to the player/object
                rb.AddForce(direction * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
