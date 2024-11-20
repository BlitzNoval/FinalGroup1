using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleJumpUpgraded : MonoBehaviour
{
    public int maxJumps = 2;  // the maximum jump count 
    private int jumpCount = 0;  // tracks the number of jumps
   // public float doubleJumpForce = 13f;
    public float superJumpForce = 20f;  // super jump force
    public float chargeTime = 4f;  // the time required to fully charge the super jump
    public float chargeRetentionTime = 10f;  // the duration to retain the full charge of the super jump
    private float chargeTimer = 0f;  // charge time for the super jump
    private float chargeRetentionTimer = 0f;
    private bool isCharging = false;
    private bool canSuperJump = false;
    private bool hasSuperJumped = false;

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    // UI Elements
    public GameObject doubleJumpUpgradeUI;  // UI for the normal Double Jump
    public GameObject superJumpUpgradeUI;   // UI for the Super Jump upgrade
    public TextMeshProUGUI chargeProgressText; // TextMeshPro to show charging progress
    public GameObject chargeBar;  // A UI element like a progress bar to show the charge

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
            // The player can perform the super jump if fully charged and has not super jumped yet
            if (canSuperJump && !hasSuperJumped)
            {
                PerformSuperJump();
                hasSuperJumped = true;
                ResetJumpingAndCharging();
            }
            else if (jumpCount < maxJumps && !playerMovement.wallrunning && !hasSuperJumped)
            {
                PerformJump();
                jumpCount++;
            }
        }

        // Reset the jumps if the player is grounded
        if (playerMovement.grounded && jumpCount > 0)
        {
            jumpCount = 0;
            hasSuperJumped = false;
        }
    }

    void HandleSuperJumpCharging()
    {
        if (playerMovement.grounded && jumpCount == 0 && !playerMovement.wallrunning && !canSuperJump)
        {
            if (Input.GetKey(KeyCode.LeftControl))  // Begin charging if the left control key is held down
            {
                isCharging = true;

                // Add to the charge time only if it's not fully charged yet
                chargeTimer += Time.deltaTime;

                // Calculate the charge percentage and stop if the percentage reaches 100
                float chargePercentage = Mathf.Clamp01(chargeTimer / chargeTime) * 100f;
                Debug.Log("Charging super jump... " + chargePercentage.ToString("F1") + "%");

                // Show the charge progress in the UI (TextMeshPro or progress bar)
                if (chargeProgressText != null)
                {
                    chargeProgressText.text = "Charge: " + Mathf.RoundToInt(chargePercentage) + "%";
                }

                // Update progress bar if it's set
                if (chargeBar != null)
                {
                    // Assuming chargeBar has a slider or image fill
                    chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = Mathf.Clamp01(chargeTimer / chargeTime);
                }

                // The super jump is fully charged and the retention timer starts
                if (chargeTimer >= chargeTime)
                {
                    canSuperJump = true;
                    chargeRetentionTimer = chargeRetentionTime;
                    Debug.Log("Super jump fully charged!");
                }
            }
            else if (isCharging)  // Reset the charge if it's interrupted before full charging
            {
                Debug.Log("Charging interrupted.");
                ResetJumpingAndCharging();
            }
        }

        // Manage the charge retention countdown when fully charged
        if (canSuperJump && chargeRetentionTimer > 0)
        {
            chargeRetentionTimer -= Time.deltaTime;

            if (chargeRetentionTimer <= 0)
            {
                Debug.Log("Charge expired.");
                ResetJumpingAndCharging();
            }
        }
        else if (!playerMovement.grounded)
        {
            ResetJumpingAndCharging();
        }
    }

    void PerformJump()
    {
        // Reset the Y velocity for consistent jumps
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply the jump force
        float jumpForce = (jumpCount == 0) ? playerMovement.jumpForce : playerMovement.jumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void PerformSuperJump()
    {
        // Reset the Y velocity for consistent super jump 
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply the super jump force
        rb.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);

        // Stop the player from jumping after super jumping until grounded
        jumpCount = maxJumps;
    }

    void ResetJumpingAndCharging()
    {
        isCharging = false;
        chargeTimer = 0f;
        chargeRetentionTimer = 0f;
        canSuperJump = false;

        // Reset the UI elements
        if (chargeProgressText != null) chargeProgressText.text = "Charge: 0%";
        if (chargeBar != null) chargeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f;
    }

    public void UpgradeToSuperJump()
    {
        // Manage UI visibility: hide the Double Jump UI and show the Super Jump UI
        if (doubleJumpUpgradeUI != null) doubleJumpUpgradeUI.SetActive(false);
        if (superJumpUpgradeUI != null) superJumpUpgradeUI.SetActive(true);
    }
}