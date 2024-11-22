using UnityEngine;

public class TeleportToGameObject : MonoBehaviour
{
    // Reference to the GameObject where the player will be teleported
    public GameObject teleportTarget;

    // OnTriggerEnter is called when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Ensure the teleport target is set
            if (teleportTarget != null)
            {
                // Teleport the player to the teleport target's position
                other.transform.position = teleportTarget.transform.position;

                // Optional: Reset the player's velocity if they have a Rigidbody
                Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.velocity = Vector3.zero;
                    playerRigidbody.angularVelocity = Vector3.zero;
                }

                // Log the teleportation event (optional)
                Debug.Log("Player teleported to: " + teleportTarget.name);
            }
            else
            {
                Debug.LogWarning("Teleport target is not set!");
            }
        }
    }
}
