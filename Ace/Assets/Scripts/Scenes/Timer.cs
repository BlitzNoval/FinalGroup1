using UnityEngine;
using TMPro;

public class SpeedrunClockWithSplits : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;       // Main timer display
    [SerializeField] private TextMeshProUGUI splitLogText;    // Split log display
    private float timer = 0f;                                 // Time elapsed
    private bool isRunning = false;                          // Timer running state
    private float lastCheckpointTime = 0f;                   // Time of the last checkpoint
    private float[] bestSplits;                              // Best splits for the scene
    private int checkpointIndex = 0;                         // Current checkpoint index

    void Start()
    {
        // Initialize best splits with a large value
        bestSplits = new float[10]; // Adjust size based on the number of checkpoints
        for (int i = 0; i < bestSplits.Length; i++)
        {
            bestSplits[i] = float.MaxValue;
        }
    }

    void Update()
    {
        if (isRunning)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Format and display the time
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt((timer % 1) * 1000);
            timerText.text = $"{minutes}:{seconds:00}.{milliseconds:000}";
        }
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        isRunning = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the timer and checkpoints.
    /// </summary>
    public void ResetTimer()
    {
        isRunning = false;
        timer = 0f;
        lastCheckpointTime = 0f;
        checkpointIndex = 0;
        timerText.text = "0:00.000";
        splitLogText.text = ""; // Clear split log
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player hits a checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            LogSplit();
        }
    }

    /// <summary>
    /// Logs a split when a checkpoint is reached.
    /// </summary>
    private void LogSplit()
    {
        if (checkpointIndex >= bestSplits.Length) return;

        // Calculate current split time
        float splitTime = timer - lastCheckpointTime;
        lastCheckpointTime = timer;

        // Compare with the best split
        bool isFaster = splitTime < bestSplits[checkpointIndex];
        float difference = splitTime - bestSplits[checkpointIndex];

        // Update best split if it's faster
        if (isFaster)
        {
            bestSplits[checkpointIndex] = splitTime;
        }

        // Format the split log
        string color = isFaster ? "#00FF00" : "#FF0000"; // Green for faster, red for slower
        string differenceText = isFaster
            ? $"-{Mathf.Abs(difference):0.000}s"
            : $"+{Mathf.Abs(difference):0.000}s";

        // Append split time to the log
        splitLogText.text += $"Checkpoint {checkpointIndex + 1}: {splitTime:0.000}s <color={color}>{differenceText}</color>\n";

        // Increment checkpoint index
        checkpointIndex++;
    }
}
