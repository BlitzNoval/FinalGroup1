using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJump : MonoBehaviour
{
    public int extraJumps = 1; // Number of extra jumps allowed (e.g., one double jump)
    private int remainingJumps; // Tracks the remaining extra jumps
    public float doubleJumpForce = 13f; // Force applied for the double jump

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    // UI elements
    public GameObject blackoutUI; // UI to indicate the double jump is unavailable

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        remainingJumps = extraJumps; // Initialize remaining jumps to the allowed extra jumps
    }

    void Update()
    {
        // Check if the jump key is pressed and the player has remaining extra jumps
        if (Input.GetKeyDown(playerMovement.jumpKey) && remainingJumps > 0 && !playerMovement.wallrunning && !playerMovement.grounded)
        {
            PerformDoubleJump();
            remainingJumps--; // Consume one extra jump
        }

        // Reset the remaining extra jumps when grounded or wall-running
        if (playerMovement.grounded || playerMovement.wallrunning)
        {
            remainingJumps = extraJumps;
        }

        // Update UI based on remaining jumps
        UpdateJumpUI();
    }

    void PerformDoubleJump()
    {
        // Reset Y velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply the double jump force
        rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
    }

    private void UpdateJumpUI()
    {
        // Enable the blackout UI if no extra jumps are left
        if (remainingJumps <= 0)
        {
            if (blackoutUI != null) blackoutUI.SetActive(true);
        }
        else
        {
            if (blackoutUI != null) blackoutUI.SetActive(false);
        }
    }
}
