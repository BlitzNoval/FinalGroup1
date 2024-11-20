using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float levelTime = 120f; // Total countdown time in seconds
    private float currentTime;
    private Transform startCheckpoint;

    // Checkpoint timing
    private float[] checkpointTimes;
    private float[] bestCheckpointTimes; // Stored best times

    // UI Elements
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI splitText;

    private bool timerRunning = false;
    private int currentCheckpointIndex = 0;

    void Start()
    {
        // Load best times from PlayerPrefs
        LoadBestTimes();

        // Initialize the timer and checkpoint array
        currentTime = levelTime;
        checkpointTimes = new float[bestCheckpointTimes.Length];
        timerRunning = true;

        // Assume the player starts at the first checkpoint
        PlayerRespawn playerRespawn = FindObjectOfType<PlayerRespawn>();
        if (playerRespawn != null)
        {
            startCheckpoint = playerRespawn.GetLastCheckpoint(); // Use the public getter
        }
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTime <= 0)
            {
                timerRunning = false;
                ResetPlayerToStart();
            }
        }
    }

    public void OnCheckpointReached(int checkpointIndex)
    {
        // Calculate split time
        float splitTime = levelTime - currentTime;

        // Compare with best time
        float bestTime = bestCheckpointTimes[checkpointIndex];
        string splitMessage;
        if (bestTime > 0)
        {
            float timeDifference = splitTime - bestTime;
            if (timeDifference < 0)
            {
                splitMessage = $"-{Mathf.Abs(timeDifference):F2}s (Faster)";
                splitText.color = Color.green;
            }
            else
            {
                splitMessage = $"+{Mathf.Abs(timeDifference):F2}s (Slower)";
                splitText.color = Color.red;
            }
        }
        else
        {
            splitMessage = "First Time!";
            splitText.color = Color.yellow;
        }

        // Display split
        splitText.text = splitMessage;

        // Update best time if this attempt is faster
        if (bestTime == 0 || splitTime < bestTime)
        {
            bestCheckpointTimes[checkpointIndex] = splitTime;
            SaveBestTimes();
        }

        // Update checkpoint time
        checkpointTimes[checkpointIndex] = splitTime;
    }

    private void UpdateTimerUI()
    {
        timerText.text = $"Time Left: {currentTime:F2}s";
    }

    private void ResetPlayerToStart()
    {
        PlayerRespawn playerRespawn = FindObjectOfType<PlayerRespawn>();
        if (playerRespawn != null && startCheckpoint != null)
        {
            playerRespawn.transform.position = startCheckpoint.position;
            Debug.Log("Player reset to start checkpoint.");
        }
        currentTime = levelTime; // Reset timer
        timerRunning = true;    // Restart timer
    }

    private void LoadBestTimes()
    {
        int checkpointCount = FindObjectsOfType<CheckpointU>().Length;
        bestCheckpointTimes = new float[checkpointCount];

        for (int i = 0; i < checkpointCount; i++)
        {
            bestCheckpointTimes[i] = PlayerPrefs.GetFloat($"Checkpoint_{i}_BestTime", 0f);
        }
    }

    private void SaveBestTimes()
    {
        for (int i = 0; i < bestCheckpointTimes.Length; i++)
        {
            PlayerPrefs.SetFloat($"Checkpoint_{i}_BestTime", bestCheckpointTimes[i]);
        }
        PlayerPrefs.Save();
    }
}
