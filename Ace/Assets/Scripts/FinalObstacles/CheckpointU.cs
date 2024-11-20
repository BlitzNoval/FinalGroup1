using UnityEngine;

public class CheckpointU : MonoBehaviour
{
    private bool isCheckpointActivated = false;
    public GameObject respawnPoint;
    public SpeedrunTimer speedrunTimer; // Reference to the SpeedrunTimer
    private static float previousSplitTime = 0; // Time of the previous split
    private static int checkpointCounter = 0; // Counter to label checkpoints

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCheckpointActivated)
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.SetCheckpoint(respawnPoint);
                isCheckpointActivated = true;

                float currentSplitTime = speedrunTimer.GetElapsedTime();
                float splitDelta = currentSplitTime - previousSplitTime;

                checkpointCounter++; // Increment checkpoint number
                speedrunTimer.RecordSplit(currentSplitTime, splitDelta, checkpointCounter);

                previousSplitTime = currentSplitTime; // Update the previous split time
            }
        }
    }
}
