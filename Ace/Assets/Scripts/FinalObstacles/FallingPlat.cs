using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DisappearingBlockWithFeedback : MonoBehaviour
{
    [Header("Disappear Settings")]
    public float disappearDelay = 1.0f; // Time before the block disappears
    public float resetDelay = 3.0f; // Time before the block reappears
    public bool enableReset = true; // Toggle to allow the block to reappear

    [Header("Feedback Settings")]
    public bool useColorChange = true; // Enable color change feedback
    public Color warningColor = Color.red; // Color the block transitions to before disappearing
    public bool useScalingEffect = false; // Enable scaling effect feedback
    public Vector3 scaleChange = new Vector3(0.5f, 0.5f, 0.5f); // Target scale change
    public bool useGlowEffect = false; // Enable pulsating glow effect
    public float glowFrequency = 2.0f; // Frequency of the glow pulsation
    public AudioClip disappearSound; // Sound effect for disappearing
    public AudioClip countdownBeepSound; // Countdown beep sound
    public ParticleSystem disappearParticles; // Optional particle system for disappearing

    [Header("Material Settings")]
    public Material baseMaterial; // Original material for the block

    private Vector3 initialPosition; // Original position of the block
    private Quaternion initialRotation; // Original rotation of the block
    private Vector3 initialScale; // Original scale of the block
    private Renderer blockRenderer; // Renderer for visual feedback
    private Collider blockCollider; // Collider for disabling interactions
    private AudioSource audioSource;
    private bool isDisappearing = false; // To prevent multiple triggers

    private void Start()
    {
        // Cache the initial properties
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;

        // Cache the renderer and collider
        blockRenderer = GetComponent<Renderer>();
        blockCollider = GetComponent<Collider>();

        // Assign base material if it's not null
        if (baseMaterial != null)
        {
            blockRenderer.material = baseMaterial;
        }

        // Add AudioSource if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDisappearing)
        {
            isDisappearing = true;
            StartCoroutine(DisappearSequence());
        }
    }

    private IEnumerator DisappearSequence()
    {
        // Play countdown beeps
        if (countdownBeepSound != null)
        {
            StartCoroutine(PlayCountdownBeep(disappearDelay));
        }

        // Trigger feedback effects
        if (useColorChange)
        {
            StartCoroutine(ColorChangeEffect());
        }

        if (useScalingEffect)
        {
            StartCoroutine(ScalingEffect());
        }

        if (useGlowEffect)
        {
            StartCoroutine(GlowEffect());
        }

        // Wait for the disappear delay
        yield return new WaitForSeconds(disappearDelay);

        // Play disappearing feedback
        if (disappearSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(disappearSound);
        }

        if (disappearParticles != null)
        {
            disappearParticles.Play();
        }

        // Make the block disappear
        blockRenderer.enabled = false; // Hide the block visually
        blockCollider.enabled = false; // Disable interactions

        // Wait for the reset delay if enabled
        if (enableReset)
        {
            yield return new WaitForSeconds(resetDelay);
            StartCoroutine(RespawnBlock());
        }
    }

    private IEnumerator PlayCountdownBeep(float duration)
    {
        int beepCount = Mathf.FloorToInt(duration); // One beep per second
        float interval = duration / beepCount;

        for (int i = 0; i < beepCount; i++)
        {
            if (countdownBeepSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(countdownBeepSound);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator ColorChangeEffect()
    {
        Color originalColor = blockRenderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < disappearDelay)
        {
            float t = elapsedTime / disappearDelay;
            blockRenderer.material.color = Color.Lerp(originalColor, warningColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ScalingEffect()
    {
        Vector3 targetScale = initialScale + scaleChange;
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < disappearDelay)
        {
            float t = elapsedTime / disappearDelay;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator GlowEffect()
    {
        Material mat = blockRenderer.material;
        float elapsedTime = 0f;

        while (elapsedTime < disappearDelay)
        {
            float glow = Mathf.PingPong(Time.time * glowFrequency, 1f);
            mat.SetColor("_EmissionColor", warningColor * glow);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mat.SetColor("_EmissionColor", Color.black); // Turn off glow after effect
    }

    private IEnumerator RespawnBlock()
    {
        // Reset block's properties gradually
        blockRenderer.enabled = true;
        blockCollider.enabled = true;
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset material to base material
        if (baseMaterial != null)
        {
            blockRenderer.material = baseMaterial;
        }

        float elapsedTime = 0f;
        Vector3 smallScale = initialScale * 0.1f;
        transform.localScale = smallScale;

        while (elapsedTime < 1f)
        {
            float t = elapsedTime / 1f;
            transform.localScale = Vector3.Lerp(smallScale, initialScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;
        isDisappearing = false; // Allow the block to disappear again
    }
}
