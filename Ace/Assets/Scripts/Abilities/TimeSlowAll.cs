using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowAll : MonoBehaviour
{
    public float slowTimeScale = 0.5f;  // Set to half-speed
    public float normalTimeScale = 1.0f;
    public float slowDuration = 2.0f;   // Duration of the slow motion in seconds

    private float slowTimeRemaining;

    void Update()
    {
        // Trigger slow motion when player presses the 'S' key
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartSlowMotion();
        }

        // Update the remaining slow time and restore normal time when finished
        if (slowTimeRemaining > 0)
        {
            slowTimeRemaining -= Time.unscaledDeltaTime;
            if (slowTimeRemaining <= 0)
            {
                RestoreNormalTime();
            }
        }
    }

    // Function to start slow motion
    public void StartSlowMotion()
    {
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;  // Adjust physics time step
    }

    // Function to restore normal time
    public void RestoreNormalTime()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;  // Restore default physics time step
    }
}
