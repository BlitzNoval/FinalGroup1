using UnityEngine;
using TMPro;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component
    private float elapsedTime;        // Time since the timer started

    void Update()
    {
        elapsedTime += Time.deltaTime; // Increment elapsed time

        // Format the elapsed time as minutes:seconds:milliseconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        // Update the TextMeshProUGUI text
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
    public float GetElapsedTime()
{
    return elapsedTime;
}

}
