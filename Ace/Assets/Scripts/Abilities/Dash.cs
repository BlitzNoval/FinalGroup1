using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashDistance = 100f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private int maxDashCount = 3;
    [SerializeField] private float dashRechargeTime = 5f;

    private bool readyToDash = true;
    private int currentDashCount;

    public KeyCode dashKey = KeyCode.LeftShift;

    private PlayerMovement playerMovement;
    private Camera mainCamera;
    private Rigidbody rb;

    // UI elements
    public GameObject dashUI;
    public GameObject blackoutUI;

    // Audio and Particle
    [Header("Dash Feedback")]
    public AudioClip dashSound; // Drag the dash sound clip here
    private AudioSource audioSource;
    public GameObject dashParticleEffect; // Drag your particle effect here

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this GameObject");
        }

        currentDashCount = maxDashCount;

        // Ensure AudioSource is attached and ready
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Automatically add AudioSource if missing
        }

        // Ensure the particle effect is disabled at the start
        if (dashParticleEffect != null)
        {
            dashParticleEffect.SetActive(false); // Disable the particle effect initially
        }
        else
        {
            Debug.LogError("Dash particle effect is not assigned in the Inspector!");
        }

        // Ensure UI is updated at the start
        UpdateDashUI();
    }

    private void Update()
    {
        // Check if the dash key is pressed and dash is ready
        if (readyToDash && currentDashCount > 0 && !playerMovement.wallrunning && Input.GetKeyDown(dashKey))
        {
            StartCoroutine(PerformDash());
        }

        UpdateDashUI();
    }

    private IEnumerator PerformDash()
    {
        readyToDash = false;
        currentDashCount--;

        // Dash direction
        Vector3 dashDirection = mainCamera.transform.forward;
        dashDirection.y = 0; // Keep the dash horizontal
        dashDirection.Normalize();

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        // Play the dash sound
        if (dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
        else
        {
            Debug.LogError("Dash sound is not assigned in the Inspector!");
        }

        // Enable the particle effect immediately when the dash key is pressed
        if (dashParticleEffect != null)
        {
            dashParticleEffect.SetActive(true); // Immediately enable
        }

        // Wait for 0.5 seconds before disabling the particle effect
        yield return new WaitForSeconds(0.5f);

        // Disable the particle effect after 0.5 seconds
        if (dashParticleEffect != null)
        {
            dashParticleEffect.SetActive(false); // Disable the particle effect
        }

        yield return new WaitForSeconds(dashCooldown); // Wait for cooldown

        readyToDash = true;

        if (currentDashCount <= 0)
        {
            StartCoroutine(RechargeDashes());
        }
    }

    private IEnumerator RechargeDashes()
    {
        yield return new WaitForSeconds(dashRechargeTime);
        currentDashCount = maxDashCount;
    }

    private void UpdateDashUI()
    {
        dashUI.SetActive(true);

        if (currentDashCount <= 0 || !readyToDash)
        {
            blackoutUI.SetActive(true);
        }
        else
        {
            blackoutUI.SetActive(false);
        }
    }
}
