using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJump : MonoBehaviour
{
    public int maxJumps = 2;  // Allows for both jumps (normal & double jump)
    private int jumpCount = 0;  // Tracks the amount of jumps
    public float doubleJumpForce = 13f;  // The force of the double jump

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    // UI elements
    public GameObject blackoutUI;    // UI to show when double jump is unavailable

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check if the jump key is pressed, player is not grounded, and player is not wallrunning
        if (Input.GetKeyDown(playerMovement.jumpKey) && jumpCount < maxJumps && !playerMovement.wallrunning)
        {
            PerformJump();
            jumpCount++;
        }

        // Reset jump count when grounded
        if (playerMovement.grounded)
        {
            jumpCount = 0;
        }

        // Update UI based on jump status
        UpdateJumpUI();
    }

    void PerformJump()
    {
        // Reset the Y velocity for a consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply jump force
        float jumpForce = (jumpCount == 0) ? playerMovement.jumpForce : doubleJumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Update Jump UI based on current jump status
    private void UpdateJumpUI()
    {
        // Enable the DoubleJumpUI if the player can double jump (has not used the second jump yet)
        if (jumpCount < maxJumps)
        {
            blackoutUI.SetActive(false);
        }
        // Enable BlackoutUI after the second jump is used
        else
        {
            blackoutUI.SetActive(true);
        }
    }
}
