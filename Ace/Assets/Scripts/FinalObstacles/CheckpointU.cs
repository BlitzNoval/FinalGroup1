using UnityEngine;

public class CheckpointU : MonoBehaviour
{
    private bool isCheckpointActivated = false; // Flag to ensure the log happens only once

    // Public GameObject for the respawn point
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCheckpointActivated)
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.SetCheckpoint(respawnPoint); // Set the checkpoint to this GameObject
                isCheckpointActivated = true;  // Prevent re-logging this checkpoint
                Debug.Log("Checkpoint activated at: " + transform.position);
            }
        }
    }
}