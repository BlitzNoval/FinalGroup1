using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TimeSlow : MonoBehaviour
{
    public float slowTimeScale = 0.5f;      // scale for slowing down time
    public float normalTimeScale = 1.0f;    // the normal time scale
    public float slowDuration = 2.0f;       // the duration of the slow-motion effect
    public float playerMultiply = 0.5f;

    private float slowTimeRemaining;        // the time remaining for the slow-motion effect
    private PlayerMovement playerMovement;

    private bool isSlowMotionActive = false;

    // UI elements
    public GameObject blackoutUI;           // UI to show when slow motion is active
    public TextMeshProUGUI slowDurationText; // TextMeshPro for displaying the duration of the ability

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }

        // Initially hide the blackout UI
        blackoutUI.SetActive(false);
    }

    private void Update()
    {
        // Check for activation of slow motion ability (F key)
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartSlowMotion();
        }

        // If slow motion is active, update remaining time and UI
        if (isSlowMotionActive)
        {
            if (slowTimeRemaining > 0)
            {
                slowTimeRemaining -= Time.unscaledDeltaTime;

                // Update the TextMeshPro UI with the remaining time
                slowDurationText.text = Mathf.Ceil(slowTimeRemaining).ToString("0") + "s";

                if (slowTimeRemaining <= 0)
                {
                    RestoreNormalTime();
                }
            }
        }
        else
        {
            // If slow motion is inactive, hide the UI
            slowDurationText.text = ""; // Clear the text when slow motion isn't active
        }
    }

    public void StartSlowMotion()
    {
        if (isSlowMotionActive) return;

        isSlowMotionActive = true;
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Show the blackout UI
        blackoutUI.SetActive(true);

        if (playerMovement != null)
        {
            // Apply a time multiplier to control player speed during slow motion
            playerMovement.ApplyTimeMultiplier(1 / playerMultiply);  // this determines the player's speed based in the slow time period
        }
    }

    public void RestoreNormalTime()
    {
        if (!isSlowMotionActive) return;

        isSlowMotionActive = false;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;

        // Hide the blackout UI when slow motion ends
        blackoutUI.SetActive(false);

        if (playerMovement != null)
        {
            // Reset the player's speed multiplier to normal
            playerMovement.ApplyTimeMultiplier(1.0f);  // resets the player back to normal
        }
    }
}
