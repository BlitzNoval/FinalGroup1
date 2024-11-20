using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform lastCheckpoint; // The last active checkpoint

    // Respawn the player at the last checkpoint (GameObject)
    public void SetCheckpoint(GameObject checkpoint)
    {
        lastCheckpoint = checkpoint.transform; // Store the checkpoint's position
        Debug.Log("Checkpoint set at: " + lastCheckpoint.position);
    }

    // Public method to get the last checkpoint
    public Transform GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public void RespawnAtCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Reset velocity and angular velocity
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Reset player position to the last checkpoint's position
            transform.position = lastCheckpoint.position;
            Debug.Log("Player respawned at checkpoint: " + lastCheckpoint.position);
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Player respawn failed.");
        }
    }
}
