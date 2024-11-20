using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashDistance = 100f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private int maxDashCount = 3; // the number of maximum dashes
    [SerializeField] private float dashRechargeTime = 5f; // time to fully recharge dashes

    private bool readyToDash = true; // checks if the dash is ready
    private int currentDashCount; // check on the amount of dashes available

    public KeyCode dashKey = KeyCode.LeftShift;

    private PlayerMovement playerMovement;
    private Camera mainCamera;
    private Rigidbody rb;

    // UI elements
    public GameObject dashUI;  // UI to show when dashing is available
    public GameObject blackoutUI; // UI to show when dashing is unavailable

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

        // Ensure UI is updated at the start
        UpdateDashUI();
    }

    private void Update()
    {
        // Check if dash is ready, dashes are available, not wallrunning, and dash key is pressed
        if (readyToDash && currentDashCount > 0 && !playerMovement.wallrunning && Input.GetKeyDown(dashKey))
        {
            StartCoroutine(PerformDash());
        }

        // Update UI each frame
        UpdateDashUI();
    }

    private IEnumerator PerformDash()
    {
        readyToDash = false; // Dash is set to being not ready
        currentDashCount--; // Decrease the count for the dash

        // Dash direction
        Vector3 dashDirection = mainCamera.transform.forward;
        dashDirection.y = 0; // Keep the direction of the dash horizontal
        dashDirection.Normalize();

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(dashCooldown);
        readyToDash = true; // Reset the dash cooldown

        if (currentDashCount <= 0)
        {
            StartCoroutine(RechargeDashes());
        }
    }

    private IEnumerator RechargeDashes()
    {
        yield return new WaitForSeconds(dashRechargeTime);
        currentDashCount = maxDashCount; // Reset the dash count to max
    }

    // Update Dash UI based on current dash status
    private void UpdateDashUI()
    {
        // Always show DashUI
        dashUI.SetActive(true);

        // Show BlackoutUI only when dashing is unavailable
        if (currentDashCount <= 0 || !readyToDash)
        {
            blackoutUI.SetActive(true); // Show BlackoutUI if dashes are unavailable
        }
        else
        {
            blackoutUI.SetActive(false); // Hide BlackoutUI if dashes are available
        }
    }
}

