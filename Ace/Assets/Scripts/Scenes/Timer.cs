using UnityEngine; 
using TMPro; 
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.UI;

public class SpeedrunTimer : MonoBehaviour
{
  // TextMeshProUGUI for timer and temporary split display
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI splitDisplayText; 

    // Regular Text for best time and splits
    public Text splitsText; 
    public Text bestTimeText; 

    private float elapsedTime;
    private List<SplitData> splitRecords = new List<SplitData>(); // Store split data
    private bool isFinished = false; // Track if the run is completed
    private float finalTime;

    // Unique identifier for the current track/level
    public string trackIdentifier = "DefaultTrack";

    // Struct to store comprehensive split information
    [System.Serializable]
    public class SplitData
    {
        public int CheckpointNumber;
        public float SplitTime;
        public float Delta;
        public string FormattedSplit;
    }

    void Start()
    {
        // Load and display best time when the scene starts
        LoadAndDisplayBestTime();
    }

    void Update()
    {
        if (!isFinished)
        {
            elapsedTime += Time.deltaTime;

            // Format the elapsed time
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }

        // Reset PlayerPrefs when the "K" key is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            ResetPlayerPrefs();
        }
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

   public void RecordSplit(float splitTime, float delta, int checkpointNumber)
{
    // Format split time
    int minutes = Mathf.FloorToInt(splitTime / 60);
    int seconds = Mathf.FloorToInt(splitTime % 60);
    int milliseconds = Mathf.FloorToInt((splitTime * 1000) % 1000);

    // Determine delta sign and format
    string deltaSign = delta >= 0 ? "+" : "-";
    float absDelta = Mathf.Abs(delta);

    // Format delta time
    int deltaMinutes = Mathf.FloorToInt(absDelta / 60);
    int deltaSeconds = Mathf.FloorToInt(absDelta % 60);
    int deltaMilliseconds = Mathf.FloorToInt((absDelta * 1000) % 1000);

    string deltaFormatted = string.Format("{0}{1:00}:{2:00}:{3:000}", deltaSign, deltaMinutes, deltaSeconds, deltaMilliseconds);

    // Create formatted split entry
    string splitEntry = $"Checkpoint {checkpointNumber}: {minutes:00}:{seconds:00}:{milliseconds:000} ({deltaFormatted})";

    // Save current split time to PlayerPrefs
    string splitKey = $"{trackIdentifier}_Checkpoint_{checkpointNumber}";
    float bestSplitTime = PlayerPrefs.GetFloat(splitKey, float.MaxValue);

    // Compare current split time with the best split time
    if (splitTime < bestSplitTime)
    {
        PlayerPrefs.SetFloat(splitKey, splitTime);
        PlayerPrefs.Save();
        splitEntry = $"<color=green>{splitEntry}</color>"; // Green if faster
    }
    else
    {
        splitEntry = $"<color=red>{splitEntry}</color>"; // Red if slower
    }

    // Store split record with color
    SplitData newSplit = new SplitData
    {
        CheckpointNumber = checkpointNumber,
        SplitTime = splitTime,
        Delta = delta,
        FormattedSplit = splitEntry  // Store the colored entry
    };

    splitRecords.Add(newSplit);

    // Update permanent splits text
    UpdatePermanentSplitsDisplay();

    // Show temporary split display
    StartCoroutine(ShowTemporarySplitDisplay(splitEntry));
}
    public void FinishRun()
    {
        if (!isFinished)
        {
            isFinished = true;
            finalTime = elapsedTime;

            // Format final time
            int minutes = Mathf.FloorToInt(finalTime / 60);
            int seconds = Mathf.FloorToInt(finalTime % 60);
            int milliseconds = Mathf.FloorToInt((finalTime * 1000) % 1000);

            string finalTimeFormatted = $"Final Time: {minutes:00}:{seconds:00}:{milliseconds:000}";

            // Add final time to splits
            splitRecords.Add(new SplitData
            {
                CheckpointNumber = 0,
                SplitTime = finalTime,
                Delta = 0,
                FormattedSplit = finalTimeFormatted
            });

            // Update splits display
            UpdatePermanentSplitsDisplay();

            // Check and update best time
            CheckAndUpdateBestTime(finalTime);

            // Show final time temporarily
            StartCoroutine(ShowTemporarySplitDisplay(finalTimeFormatted));
        }
    }

    private void CheckAndUpdateBestTime(float currentTime)
    {
        // Get the current best time for this track
        float bestTime = PlayerPrefs.GetFloat(trackIdentifier + "_BestTime", float.MaxValue);

        // Check if current time is better (lower is better)
        if (currentTime < bestTime)
        {
            // Save new best time
            PlayerPrefs.SetFloat(trackIdentifier + "_BestTime", currentTime);
            PlayerPrefs.Save();

            // Update best time display
            UpdateBestTimeDisplay(currentTime);

            // Optional: You might want to save additional info like date, etc.
            PlayerPrefs.SetString(trackIdentifier + "_BestTimeDate", System.DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }

    private void LoadAndDisplayBestTime()
    {
        // Retrieve best time for this track
        float bestTime = PlayerPrefs.GetFloat(trackIdentifier + "_BestTime", 0);

        // Update best time display if a best time exists
        if (bestTime > 0)
        {
            UpdateBestTimeDisplay(bestTime);
        }
        else
        {
            // No best time set yet
            bestTimeText.text = "--:--:---";
        }
    }

    private void UpdateBestTimeDisplay(float bestTime)
    {
        // Format best time
        int minutes = Mathf.FloorToInt(bestTime / 60);
        int seconds = Mathf.FloorToInt(bestTime % 60);
        int milliseconds = Mathf.FloorToInt((bestTime * 1000) % 1000);

        bestTimeText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    private void UpdatePermanentSplitsDisplay()
{
    splitsText.text = string.Join("\n", splitRecords.ConvertAll(split => split.FormattedSplit));
}

    private IEnumerator ShowTemporarySplitDisplay(string splitText)
    {
        // Show split on temporary display
        splitDisplayText.text = splitText;
        splitDisplayText.gameObject.SetActive(true);

        // Wait for 5 seconds (increased from 3 seconds)
        yield return new WaitForSeconds(5f);

        // Hide split display
        splitDisplayText.gameObject.SetActive(false);
    }

    // Method to retrieve stored split records
    public List<SplitData> GetSplitRecords()
    {
        return new List<SplitData>(splitRecords);
    }

    // Optional: Method to reset best time (can be called from a UI button)
    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey(trackIdentifier + "_BestTime");
        PlayerPrefs.DeleteKey(trackIdentifier + "_BestTimeDate");
        bestTimeText.text = "Best Time: --:--:---";
    }

    // Method to reset all PlayerPrefs (called when "K" is pressed)
    private void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        LoadAndDisplayBestTime(); // Reload best time after reset
        splitRecords.Clear();
        splitsText.text = string.Empty;
        splitDisplayText.text = string.Empty;
    }
}
