using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CheckpointU : MonoBehaviour
{
    private bool isCheckpointActivated = false; // Flag to ensure the checkpoint triggers only once
    public GameObject respawnPoint;            // Reference to the respawn point

    [Header("Global Timer & Split UI")]
    public SpeedrunTimer timerScript;          // Reference to the SpeedrunTimer script (shared across checkpoints)
    public TextMeshProUGUI splitDisplay;       // Reference to TextMeshPro for displaying splits (shared across checkpoints)

    // Static list to store splits globally across checkpoints
    private static List<string> splits = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCheckpointActivated)
        {
            // Activate checkpoint
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.SetCheckpoint(respawnPoint); // Set the checkpoint
                isCheckpointActivated = true; // Prevent multiple triggers
                CaptureSplit();              // Capture the split time
            }
        }
    }

    private void CaptureSplit()
    {
        if (timerScript == null || splitDisplay == null) return;

        // Format the current elapsed time
        float elapsedTime = timerScript.GetElapsedTime();
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        string splitTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        splits.Add(splitTime); // Add the split time to the global list

        // Update the splits display globally
        UpdateSplitDisplay();
    }

    private void UpdateSplitDisplay()
    {
        splitDisplay.text = "Splits:\n"; // Clear existing text

        foreach (string split in splits)
        {
            splitDisplay.text += split + "\n"; // Append each split
        }
    }
}
