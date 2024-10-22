using UnityEngine;

public class ResetBlock : MonoBehaviour
{
    // Reference to the CheckpointAndResetSystem
    public CheckpointAndResetSystem checkpointSystem;

    // This method is called when the player touches the reset block (hazard)
    public void OnTriggerEnter(Collider other)
    {
        // Check if the player touched the reset block
        if (other.CompareTag("Player"))
        {
            // Check if a checkpoint has been set
            if (checkpointSystem.HasCheckpoint())
            {
                // Respawn the player at the last checkpoint
                checkpointSystem.RespawnPlayer();
            }
            else
            {
                Debug.Log("No checkpoint set! Unable to respawn player.");
            }
        }
    }
}