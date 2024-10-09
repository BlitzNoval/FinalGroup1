using UnityEngine;

public class Projectile : MonoBehaviour
{
    private CheckpointAndResetSystem checkpointSystem;

    public void SetCheckpointSystem(CheckpointAndResetSystem system)
    {
        checkpointSystem = system;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (checkpointSystem != null)
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
            else
            {
                Debug.LogError("CheckpointAndResetSystem not set on projectile!");
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
    }
}