using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public float slowTimeScale = 0.5f;      // scale for slowing down time 
    public float normalTimeScale = 1.0f;    // the normal time scale
    public float slowDuration = 2.0f;        
    public float playerMultiply = 0.5f;

    private float slowTimeRemaining;          
    private PlayerMovement playerMovement;    

    private bool isSlowMotionActive = false;  

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartSlowMotion();
        }

        if (slowTimeRemaining > 0)
        {
            slowTimeRemaining -= Time.unscaledDeltaTime;
            if (slowTimeRemaining <= 0)
            {
                RestoreNormalTime();
            }
        }
    }

    public void StartSlowMotion()
    {
        if (isSlowMotionActive) return;

        isSlowMotionActive = true;
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (playerMovement != null)
        {
            // apply a time multiplier to control player speed during slow motion
            playerMovement.ApplyTimeMultiplier(1 / playerMultiply);  // this determines the player's speed based in the slow time period
        }
    }

    public void RestoreNormalTime()
    {
        if (!isSlowMotionActive) return;

        isSlowMotionActive = false;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;

        if (playerMovement != null)
        {

            playerMovement.ApplyTimeMultiplier(1.0f);  // resets the player back to normal
        }
    }
}
