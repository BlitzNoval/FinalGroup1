using UnityEngine;

public class CheckpointU : MonoBehaviour
{
    private bool isCheckpointActivated = false;
    public GameObject respawnPoint;
    public int checkpointIndex; // Unique index for each checkpoint

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCheckpointActivated)
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            TimerManager timerManager = FindObjectOfType<TimerManager>();

            if (playerRespawn != null && timerManager != null)
            {
                playerRespawn.SetCheckpoint(respawnPoint);
                timerManager.OnCheckpointReached(checkpointIndex);

                isCheckpointActivated = true;
                Debug.Log($"Checkpoint {checkpointIndex} activated.");
            }
        }
    }
}
