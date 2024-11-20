using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleJumpUpgraded : MonoBehaviour
{
    public int maxJumps = 2; // Maximum jump count
    private int jumpCount = 0; // Tracks the number of jumps
    public float jumpForce = 10f; // Normal jump force (editable in Inspector)
    public float superJumpForce = 20f; // Super jump force
    public float chargeTime = 4f; // Time required to fully charge the super jump
    public float chargeRetentionTime = 10f; // Duration to retain the full charge of the super jump
    private float chargeTimer = 0f; // Charge timer for the super jump
    private float chargeRetentionTimer = 0f;
    private bool isCharging = false;
    private bool canSuperJump = false;
    private bool hasSuperJumped = false;

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    // UI Elements
    public GameObject chargeBar; // UI element for charge progress bar
    public GameObject blackoutUI; // UI element for blackout effect

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleSuperJumpCharging();

        if (Input.GetKeyDown(playerMovement.jumpKey))
        {
            if (canSuperJump && !hasSuperJumped)
            {
                PerformSuperJump();
                hasSuperJumped = true;
                EnableBlackoutUI();
                ResetChargeBar(); // Reset charge bar after using the charged jump
            }
            else if (jumpCount < maxJumps && !playerMovement.wallrunning && !hasSuperJumped)
            {
                PerformJump();
                jumpCount++;
                if (jumpCount >= maxJumps) EnableBlackoutUI();
            }
        }

        // Reset jumps and UI when grounded or wall-running
        if ((playerMovement.grounded || playerMovement.wallrunning) && jumpCount > 0)
        {
            jumpCount = 0;
            hasSuperJumped = false;
            DisableBlackoutUI();
        }
    }

    void HandleSuperJumpCharging()
    {
        if (playerMovement.grounded && jumpCount == 0 && !playerMovement.wallrunning && !canSuperJump)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isCharging = true;
                chargeTimer += Time.deltaTime;

                if (chargeBar != null)
                {
                    // Update the charge bar based on the charging progress
                    chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = Mathf.Clamp01(chargeTimer / chargeTime);
                }

                if (chargeTimer >= chargeTime)
                {
                    canSuperJump = true;
                    chargeRetentionTimer = chargeRetentionTime;
                }
            }
            else if (isCharging)
            {
                ResetJumpingAndCharging();
            }
        }

        if (canSuperJump && chargeRetentionTimer > 0)
        {
            chargeRetentionTimer -= Time.deltaTime;

            if (chargeRetentionTimer <= 0) ResetJumpingAndCharging();
        }
    }

    void PerformJump()
    {
        // Reset Y velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Use the edited jumpForce directly from this script (not playerMovement)
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void PerformSuperJump()
    {
        // Reset Y velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply super jump force
        rb.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);
        jumpCount = maxJumps; // Prevent further jumps until grounded
    }

    void ResetJumpingAndCharging()
    {
        isCharging = false;
        chargeTimer = 0f;
        chargeRetentionTimer = 0f;
        canSuperJump = false;

        if (chargeBar != null)
            chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f; // Reset the charge bar
    }

    void ResetChargeBar()
    {
        if (chargeBar != null)
            chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f; // Clear the charge bar
    }

    void EnableBlackoutUI()
    {
        if (blackoutUI != null) blackoutUI.SetActive(true);
    }

    void DisableBlackoutUI()
    {
        if (blackoutUI != null) blackoutUI.SetActive(false);
    }
}