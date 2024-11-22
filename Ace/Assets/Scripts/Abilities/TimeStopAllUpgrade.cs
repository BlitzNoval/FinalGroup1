using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeStopAllUpgrade : MonoBehaviour
{
    public float slowTimeScale = 0.5f;  // Scale for slowing down time
    public float normalTimeScale = 1.0f;
    public float slowDuration = 2.0f;   // The period of the slow motion in seconds

    private float slowTimeRemaining;
    private bool isSlowMotionActive = false;

    // UI Elements
    public GameObject timeSlowUpgradeUI;      // UI for the Time Slow Upgrade
    public GameObject timeStopAllUpgradeUI;   // UI for the upgraded Time Slow
    public TextMeshProUGUI timeRemainingText; // TextMeshPro UI to show remaining time

    // Vignette images
    public GameObject scaleVignetteImage;  // The vignette image for scaling
    public GameObject fadeVignetteImage;   // The vignette image for fading

    private Vector3 largeScale = new Vector3(1.5f, 1.5f, 1f);
    private Vector3 normalScale = Vector3.one;
    private CanvasGroup fadeVignetteCanvasGroup; // To control the alpha of the fading vignette

    private void Start()
    {
        // Setup scale vignette image
        if (scaleVignetteImage != null)
        {
            scaleVignetteImage.SetActive(false);
            scaleVignetteImage.transform.localScale = largeScale; // Start with large scale
        }

        // Setup fade vignette image
        if (fadeVignetteImage != null)
        {
            fadeVignetteCanvasGroup = fadeVignetteImage.GetComponent<CanvasGroup>();
            if (fadeVignetteCanvasGroup == null)
            {
                fadeVignetteCanvasGroup = fadeVignetteImage.AddComponent<CanvasGroup>();
            }

            fadeVignetteImage.SetActive(false);
            fadeVignetteCanvasGroup.alpha = 0f; // Start fully transparent
        }
    }

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

            // Update the TextMeshPro UI to show the remaining time
            if (timeRemainingText != null)
            {
                timeRemainingText.text = Mathf.Max(0, slowTimeRemaining).ToString("F2") + "s";
            }
        }
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isSlowMotionActive = true;

        // Enable the timeRemainingText when slow motion starts
        if (timeRemainingText != null)
        {
            timeRemainingText.gameObject.SetActive(true); // Show the text
        }

        // Start the scale vignette effect
        if (scaleVignetteImage != null)
        {
            StartCoroutine(ActivateScaleVignette());
        }

        // Start the fade vignette effect
        if (fadeVignetteImage != null)
        {
            StartCoroutine(ActivateFadeVignette());
        }

        // Manage UI visibility: hide the old UI and show the upgraded UI
        if (timeSlowUpgradeUI != null) timeSlowUpgradeUI.SetActive(false);
        if (timeStopAllUpgradeUI != null) timeStopAllUpgradeUI.SetActive(true);
    }

    private IEnumerator ActivateScaleVignette()
    {
        scaleVignetteImage.SetActive(true); // Enable the vignette
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled delta time since time scale is altered
            float progress = elapsedTime / 0.2f;

            // Lerp from large to normal scale
            scaleVignetteImage.transform.localScale = Vector3.Lerp(largeScale, normalScale, progress);

            yield return null;
        }

        scaleVignetteImage.transform.localScale = normalScale; // Ensure final scale is set
    }

    private IEnumerator ActivateFadeVignette()
    {
        fadeVignetteImage.SetActive(true); // Enable the vignette
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / 0.2f;

            // Lerp alpha from 0 to 0.4
            fadeVignetteCanvasGroup.alpha = Mathf.Lerp(0f, 0.4f, progress);

            yield return null;
        }

        fadeVignetteCanvasGroup.alpha = 0.4f; // Ensure final alpha is set
    }

    public void RestoreNormalTime()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;
        isSlowMotionActive = false;

        // Hide the timeRemainingText when slow motion ends
        if (timeRemainingText != null)
        {
            timeRemainingText.gameObject.SetActive(false); // Hide the text
        }

        // Reset the UI visibility: Show the original UI
        if (timeSlowUpgradeUI != null) timeSlowUpgradeUI.SetActive(true);
        if (timeStopAllUpgradeUI != null) timeStopAllUpgradeUI.SetActive(false);

        // Reset vignette effects
        if (scaleVignetteImage != null)
        {
            scaleVignetteImage.SetActive(false);
            scaleVignetteImage.transform.localScale = largeScale; // Reset scale for the next activation
        }

        if (fadeVignetteImage != null)
        {
            fadeVignetteImage.SetActive(false);
            fadeVignetteCanvasGroup.alpha = 0f; // Reset alpha for the next activation
        }

        // Increase the slowDuration by 1.25x after each use
        slowDuration *= 1.25f;
    }
}
