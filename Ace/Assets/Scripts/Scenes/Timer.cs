using UnityEngine;
using TMPro;
using System.Collections;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            DisplayTime(elapsedTime);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
        StartCoroutine(CelebrateFinish());
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        DisplayTime(elapsedTime);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTimer();
            Debug.Log("Timer Stopped!");
        }
    }

    IEnumerator CelebrateFinish()
    {
        // Pulse animation
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(PulseText(1.2f, 0.2f));
            yield return StartCoroutine(PulseText(1f, 0.2f));
        }

        // Color change animation
        yield return StartCoroutine(ChangeColor(Color.red, 0.5f));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(ChangeColor(Color.yellow, 0.5f));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(ChangeColor(Color.green, 0.5f));

        // Final celebratory animation
        yield return StartCoroutine(RotateText(360f, 1f));
        yield return StartCoroutine(ScaleText(1.5f, 0.5f));
    }

    IEnumerator PulseText(float targetScale, float duration)
    {
        Vector3 originalScale = timerText.transform.localScale;
        Vector3 targetScaleVector = Vector3.one * targetScale;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            timerText.transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timerText.transform.localScale = originalScale;
    }

    IEnumerator ChangeColor(Color targetColor, float duration)
    {
        Color originalColor = timerText.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            timerText.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timerText.color = targetColor;
    }

    IEnumerator RotateText(float angle, float duration)
    {
        Quaternion originalRotation = timerText.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle) * originalRotation;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            timerText.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timerText.transform.rotation = targetRotation;
    }

    IEnumerator ScaleText(float targetScale, float duration)
    {
        Vector3 originalScale = timerText.transform.localScale;
        Vector3 targetScaleVector = Vector3.one * targetScale;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            timerText.transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timerText.transform.localScale = targetScaleVector;
    }
}