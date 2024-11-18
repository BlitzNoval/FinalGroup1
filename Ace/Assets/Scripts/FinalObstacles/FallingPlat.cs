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
    public ParticleSystem disappearParticles; // Optional particle system for disappearing

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

        // Add AudioSource if disappearing sound is provided
        if (disappearSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = disappearSound;
        }
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
            audioSource.Play();
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
            ResetBlock();
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

    private void ResetBlock()
    {
        // Reset block's visibility, collider, and properties
        blockRenderer.enabled = true;
        blockCollider.enabled = true;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        // Reset material color if color change was used
        if (useColorChange)
        {
            blockRenderer.material.color = Color.white; // Assuming white as the original color
        }

        isDisappearing = false; // Allow the block to disappear again
    }
}