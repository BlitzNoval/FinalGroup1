using UnityEngine;

public class CheckpointU : MonoBehaviour
{
    private bool isCheckpointActivated = false;
    public GameObject respawnPoint;
    public SpeedrunTimer speedrunTimer; // Reference to the SpeedrunTimer
    private static float previousSplitTime = 0; // Time of the previous split
    private static int checkpointCounter = 0; // Counter to label checkpoints

    [Header("Checkpoint Effects")]
    public AudioClip checkpointSound; // Sound to play when activated
    private AudioSource audioSource;

    private void Start()
    {
        // Ensure there's an AudioSource attached or add one
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        if (checkpointSound != null)
        {
            audioSource.clip = checkpointSound;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCheckpointActivated)
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                // Set the checkpoint and update split time
                playerRespawn.SetCheckpoint(respawnPoint);
                isCheckpointActivated = true;

                float currentSplitTime = speedrunTimer.GetElapsedTime();
                float splitDelta = currentSplitTime - previousSplitTime;

                checkpointCounter++; // Increment checkpoint number
                speedrunTimer.RecordSplit(currentSplitTime, splitDelta, checkpointCounter);

                previousSplitTime = currentSplitTime; // Update the previous split time

                // Trigger sound effect
                TriggerSoundEffect();
            }
        }
    }

    private void TriggerSoundEffect()
    {
        // Play sound effect
        if (audioSource != null && checkpointSound != null)
        {
            audioSource.Play();
        }
    }
}
