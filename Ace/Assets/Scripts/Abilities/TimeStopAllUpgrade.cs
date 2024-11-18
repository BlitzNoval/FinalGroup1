using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopAllUpgrade : MonoBehaviour
{
    public float slowTimeScale = 0.5f;  // scale for slowing down time 
    public float normalTimeScale = 1.0f;
    public float slowDuration = 2.0f;   // the period of the slow motion in seconds

    private float slowTimeRemaining;
    private bool isSlowMotionActive = false;  

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
        }
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;  
        isSlowMotionActive = true; 


        slowDuration *= 2;
    }


    public void RestoreNormalTime()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f; 
        isSlowMotionActive = false;  
    }
}
