using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashUpgrade : MonoBehaviour
{
    [SerializeField] private float dashDistance = 10f; // Total horizontal distance of the dash
    [SerializeField] private float dashSpeed = 50f; // Speed for the dash
    [SerializeField] private int maxStamina = 3; // Maximum number of dashes
    [SerializeField] private float staminaRechargeTime = 2f; // Time to recharge each stamina bar
    [SerializeField] private float verticalDashScale = 0.1f; // Vertical dash is 1/10th of horizontal

    public KeyCode dashKey = KeyCode.LeftShift;

    private int currentStamina; // Current number of dashes available
    private bool isRecharging = false; // Whether stamina recharge is active
    private Camera mainCamera;
    private Rigidbody rb;

    // UI elements
    public GameObject blackoutUI; // UI to show when dashes are unavailable
    public TextMeshProUGUI dashCountText; // TextMeshPro to display current dashes

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this GameObject");
        }

        currentStamina = maxStamina;

        // Initial UI setup
        UpdateDashUI();
        blackoutUI.SetActive(false); // Start with blackout hidden
    }

    private void Update()
    {
        // Check if dash key is pressed and stamina is available
        if (currentStamina > 0 && Input.GetKeyDown(dashKey))
        {
            PerformDash();
        }

        // Update the UI every frame
        UpdateDashUI();
    }

    private void PerformDash()
    {
        currentStamina--; // Consume one stamina point

        // Get the camera's forward direction
        Vector3 dashDirection = mainCamera.transform.forward;

        // Separate horizontal and vertical components
        Vector3 horizontalDirection = new Vector3(dashDirection.x, 0, dashDirection.z).normalized;
        float verticalDirection = dashDirection.y * verticalDashScale;

        // Combine horizontal and scaled vertical components
        Vector3 finalDashDirection = (horizontalDirection * dashDistance) + (Vector3.up * verticalDirection * dashDistance);

        // Apply force
        rb.AddForce(finalDashDirection * dashSpeed, ForceMode.Impulse);

        // Start recharging stamina if below the maximum and not already recharging
        if (currentStamina < maxStamina && !isRecharging)
        {
            StartCoroutine(RechargeStamina());
        }
    }

    private IEnumerator RechargeStamina()
    {
        isRecharging = true;

        while (currentStamina < maxStamina)
        {
            yield return new WaitForSeconds(staminaRechargeTime);
            currentStamina++; // Regenerate stamina points
        }

        isRecharging = false; // Reset recharge status when fully charged
    }

    // Update the UI elements
    private void UpdateDashUI()
    {
        // Update the dash count text
        dashCountText.text = currentStamina.ToString();

        // Show BlackoutUI when no dashes are available
        if (currentStamina == 0)
        {
            blackoutUI.SetActive(true); // Show BlackoutUI
        }
        else
        {
            blackoutUI.SetActive(false); // Hide BlackoutUI when dashes are available
        }
    }
}
