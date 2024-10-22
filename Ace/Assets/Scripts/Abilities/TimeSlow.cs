using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public float baseSpeed = 5.0f;             // Base movement speed of the player
    public float worldSlowTimeScale = 0.4f;    // Speed at which the world slows down
    public float playerTimeScale = 0.8f;       // Speed at which the player's time scale slows down
    public float normalTimeScale = 1.0f;       // Normal time scale for both world and player
    public float slowDuration = 3.0f;          // Duration of slow motion in seconds

    private float slowTimeRemaining;

    void Update()
    {
        // Activate slow motion when pressing 'F'
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartSlowMotion();
        }

        // Handle slow motion duration
        if (slowTimeRemaining > 0)
        {
            slowTimeRemaining -= Time.unscaledDeltaTime;
            if (slowTimeRemaining <= 0)
            {
                RestoreNormalTime();
            }
        }

        // Handle player movement with custom time scale
        HandlePlayerMovement();
    }

    // Start slow motion for both world and player
    public void StartSlowMotion()
    {
        // Slow down the world using timeScale
        Time.timeScale = worldSlowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics time step for world

        // Set slow motion duration
        slowTimeRemaining = slowDuration;
    }

    // Restore normal time for both world and player
    public void RestoreNormalTime()
    {
        // Restore world time scale
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;  // Restore default physics time step
    }

    // Handle player movement with independent time scale
    private void HandlePlayerMovement()
    {
        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction using a custom delta time for the player
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * baseSpeed * (Time.unscaledDeltaTime * playerTimeScale);

        // Move the player
        transform.Translate(movement);
    }
}
