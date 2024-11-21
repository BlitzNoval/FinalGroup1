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

    private Vector3 originalScale; // Store original scale for pulsing effect

    private void Start()
    {
        // Cache original scale
        originalScale = transform.localScale;

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

                // Trigger exciting effects
                TriggerExcitingEffects();
            }
        }
    }

    private void TriggerExcitingEffects()
    {
        // Play sound effect
        if (audioSource != null && checkpointSound != null)
        {
            audioSource.Play();
        }

        // Start pulse animation
        StartCoroutine(PulseScale());
    }

    private System.Collections.IEnumerator PulseScale()
    {
        float duration = 1f; // Duration of the pulse effect
        float elapsed = 0f;
        Vector3 targetScale = originalScale * 1.5f; // Target scale for pulsing

        // Scale up
        while (elapsed < duration / 2)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / (duration / 2));
            yield return null;
        }

        elapsed = 0f;

        // Scale back down
        while (elapsed < duration / 2)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / (duration / 2));
            yield return null;
        }

        // Ensure the scale is reset
        transform.localScale = originalScale;
    }
}
