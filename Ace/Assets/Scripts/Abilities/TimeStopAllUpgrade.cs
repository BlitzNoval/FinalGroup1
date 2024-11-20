using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeStopAllUpgrade : MonoBehaviour
{
    public float slowTimeScale = 0.5f;  // Scale for slowing down time
    public float normalTimeScale = 1.0f;
    public float slowDuration = 2.0f;   // The period of the slow motion in seconds

    private float slowTimeRemaining;
    private bool isSlowMotionActive = false;

    // UI Elements
    public GameObject timeSlowUpgradeUI;      // UI for the Time Slow Upgrade
    public GameObject timeStopAllUpgradeUI;   // UI for the upgraded Time Slow
    public TextMeshProUGUI timeRemainingText; // TextMeshPro UI to show remaining time


    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isSlowMotionActive)
            {
                RestoreNormalTime();
            }
            else
            {
                StartSlowMotion();
            }
        }

        if (isSlowMotionActive)
        {
            slowTimeRemaining -= Time.unscaledDeltaTime;
            if (slowTimeRemaining <= 0)
            {
                RestoreNormalTime();
            }

            // Update the TextMeshPro UI to show the remaining time
            if (timeRemainingText != null)
            {
                timeRemainingText.text = Mathf.Max(0, slowTimeRemaining).ToString("F2") + "s";
            }
        }
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isSlowMotionActive = true;

        // Enable the timeRemainingText when slow motion starts
        if (timeRemainingText != null)
        {
            timeRemainingText.gameObject.SetActive(true); // Show the text
        }

/*        // Double the slowDuration for the upgraded effect
        slowDuration *= 2;*/

        // Manage UI visibility: hide the old UI and show the upgraded UI
        if (timeSlowUpgradeUI != null) timeSlowUpgradeUI.SetActive(false);
        if (timeStopAllUpgradeUI != null) timeStopAllUpgradeUI.SetActive(true);
    }

    public void RestoreNormalTime()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;
        isSlowMotionActive = false;

        // Hide the timeRemainingText when slow motion ends
        if (timeRemainingText != null)
        {
            timeRemainingText.gameObject.SetActive(false); // Hide the text
        }

        // Reset the UI visibility: Show the original UI
        if (timeSlowUpgradeUI != null) timeSlowUpgradeUI.SetActive(true);
        if (timeStopAllUpgradeUI != null) timeStopAllUpgradeUI.SetActive(false);
    }
}
