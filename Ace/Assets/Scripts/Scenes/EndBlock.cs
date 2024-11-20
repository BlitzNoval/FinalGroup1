using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBlock : MonoBehaviour
{
    public SpeedrunTimer speedrunTimer;
    public Canvas endStatsCanvas; // Canvas to show end game stats

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the finish run method
            speedrunTimer.FinishRun();

            // Activate the end stats canvas
            if (endStatsCanvas != null)
            {
                endStatsCanvas.gameObject.SetActive(true);
            }

            // Set game time to 0 to freeze everything except mouse
            Time.timeScale = 0f;
        }
    }
}