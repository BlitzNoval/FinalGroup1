using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBlock : MonoBehaviour
{
    public SpeedrunTimer speedrunTimer;
    public Canvas endStatsCanvas; // Canvas to show end game stats
    public Canvas otherCanvas1;  // First canvas to deactivate
    public Canvas otherCanvas2;  // Second canvas to deactivate

    private void Start()
    {
        // Ensure the end stats canvas is inactive at the start of the scene
        if (endStatsCanvas != null)
        {
            endStatsCanvas.gameObject.SetActive(false);
        }
    }

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

            // Deactivate the other canvases
            if (otherCanvas1 != null)
            {
                otherCanvas1.gameObject.SetActive(false);
            }

            if (otherCanvas2 != null)
            {
                otherCanvas2.gameObject.SetActive(false);
            }

            // Set game time to 0 to freeze everything except mouse
            Time.timeScale = 0f;

            // Enable the cursor and allow it to move freely
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
