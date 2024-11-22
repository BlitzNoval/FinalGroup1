using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TimeSlow : MonoBehaviour
{
    public float slowTimeScale = 0.5f;      // scale for slowing down time
    public float normalTimeScale = 1.0f;   // the normal time scale
    public float slowDuration = 2.0f;      // the duration of the slow-motion effect
    public float playerMultiply = 0.5f;

    private float slowTimeRemaining;       // the time remaining for the slow-motion effect
    private PlayerMovement playerMovement;

    private bool isSlowMotionActive = false;

    // UI elements
    public GameObject blackoutUI;          // UI to show when slow motion is active
    public TextMeshProUGUI slowDurationText; // TextMeshPro for displaying the duration of the ability
    public GameObject scaleVignetteImage;  // The vignette image for scaling
    public GameObject fadeVignetteImage;   // The vignette image for fading

    private Vector3 largeScale = new Vector3(1.5f, 1.5f, 1f);
    private Vector3 normalScale = Vector3.one;

    private CanvasGroup fadeVignetteCanvasGroup; // To control the alpha of the fading vignette

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }

        // Initially hide the blackout UI
        blackoutUI.SetActive(false);

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

    private void Update()
    {
        // Check for activation of slow motion ability (F key)
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartSlowMotion();
        }

        // If slow motion is active, update remaining time and UI
        if (isSlowMotionActive)
        {
            if (slowTimeRemaining > 0)
            {
                slowTimeRemaining -= Time.unscaledDeltaTime;

                // Update the TextMeshPro UI with the remaining time
                slowDurationText.text = Mathf.Ceil(slowTimeRemaining).ToString("0") + "s";

                if (slowTimeRemaining <= 0)
                {
                    RestoreNormalTime();
                }
            }
        }
        else
        {
            // If slow motion is inactive, hide the UI
            slowDurationText.text = ""; // Clear the text when slow motion isn't active
        }
    }

    public void StartSlowMotion()
    {
        if (isSlowMotionActive) return;

        isSlowMotionActive = true;
        Time.timeScale = slowTimeScale;
        slowTimeRemaining = slowDuration;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Show the blackout UI
        blackoutUI.SetActive(true);

        if (playerMovement != null)
        {
            // Apply a time multiplier to control player speed during slow motion
            playerMovement.ApplyTimeMultiplier(1 / playerMultiply);
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
        if (!isSlowMotionActive) return;

        isSlowMotionActive = false;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f;

        // Hide the blackout UI when slow motion ends
        blackoutUI.SetActive(false);

        if (playerMovement != null)
        {
            // Reset the player's speed multiplier to normal
            playerMovement.ApplyTimeMultiplier(1.0f);
        }

        // Reset scale vignette
        if (scaleVignetteImage != null)
        {
            scaleVignetteImage.SetActive(false);
            scaleVignetteImage.transform.localScale = largeScale; // Reset scale for the next activation
        }

        // Reset fade vignette
        if (fadeVignetteImage != null)
        {
            fadeVignetteImage.SetActive(false);
            fadeVignetteCanvasGroup.alpha = 0f; // Reset alpha for the next activation
        }
    }
}
