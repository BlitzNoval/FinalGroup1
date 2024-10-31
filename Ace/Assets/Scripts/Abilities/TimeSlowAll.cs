using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowAll : MonoBehaviour
{
    public float slowTimeScale = 0.5f;  // scale for slowing down time 
    public float normalTimeScale = 1.0f;
    public float slowDuration = 2.0f;   

    private float slowTimeRemaining;

    void Update()
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
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;  
    }

 
    public void RestoreNormalTime()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;  
    }
}
