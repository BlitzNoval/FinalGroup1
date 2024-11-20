using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleJumpUpgraded : MonoBehaviour
{
    public int extraJumps = 2; // Number of extra jumps allowed
    private int remainingJumps; // Tracks remaining extra jumps
    public float jumpForce = 10f; // Normal jump force
    public float superJumpForce = 20f; // Force for the super jump
    public float chargeTime = 4f; // Time required to fully charge the super jump
    public float chargeRetentionTime = 10f; // Duration to retain the full charge of the super jump
    private float chargeTimer = 0f; // Timer for charging the super jump
    private float chargeRetentionTimer = 0f;
    private bool isCharging = false;
    private bool canSuperJump = false;
    private bool hasSuperJumped = false;

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    // UI Elements
    public GameObject chargeBar; // UI for charge progress
    public GameObject blackoutUI; // UI to show when jumps are unavailable

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        remainingJumps = extraJumps; // Initialize remaining jumps
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
            else if (remainingJumps > 0 && !playerMovement.wallrunning && !playerMovement.grounded && !hasSuperJumped)
            {
                PerformDoubleJump();
                remainingJumps--; // Consume one extra jump
                if (remainingJumps <= 0) EnableBlackoutUI();
            }
        }

        // Reset jumps and UI when grounded or wall-running
        if (playerMovement.grounded || playerMovement.wallrunning)
        {
            ResetJumpState();
        }
    }

    void HandleSuperJumpCharging()
    {
        if (playerMovement.grounded && !playerMovement.wallrunning && !canSuperJump)
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
                ResetChargingState();
            }
        }

        if (canSuperJump)
        {
            if (chargeRetentionTimer > 0)
            {
                chargeRetentionTimer -= Time.deltaTime;

                if (chargeBar != null)
                {
                    // Keep the charge bar full during retention
                    chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 1f;
                }
            }
            else
            {
                ResetChargingState(); // Reset when retention time expires
            }
        }
    }

    void PerformDoubleJump()
    {
        // Reset Y velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply the double jump force
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void PerformSuperJump()
    {
        // Reset Y velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply the super jump force
        rb.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);

        // Reset jump state after super jump
        remainingJumps = 0;
        canSuperJump = false;
    }

    void ResetJumpState()
    {
        remainingJumps = extraJumps;
        hasSuperJumped = false;
        DisableBlackoutUI();
    }

    void ResetChargingState()
    {
        isCharging = false;
        chargeTimer = 0f;
        chargeRetentionTimer = 0f;
        canSuperJump = false;

        if (chargeBar != null)
            chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f; // Reset charge bar
    }

    void ResetChargeBar()
    {
        if (chargeBar != null)
            chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f; // Clear charge bar
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