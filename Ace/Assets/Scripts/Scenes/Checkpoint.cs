using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Reference to the CheckpointAndResetSystem
    public CheckpointAndResetSystem checkpointSystem;

    // This method is called when the player touches the checkpoint
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that touched the checkpoint is the player
        if (other.CompareTag("Player"))
        {
            // Update the checkpoint in the checkpoint system
            checkpointSystem.UpdateCheckpoint(transform);
        }
    }
}
