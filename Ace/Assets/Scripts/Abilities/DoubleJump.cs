using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public int maxJumps = 2;  // Allows for two jumps (normal + double jump)
    private int jumpCount = 0;  // Tracks the number of jumps
    public float doubleJumpForce = 13f;  // The force applied for the double jump

    private Rigidbody rb;
    private PlayerMovement playerMovement;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check if the jump key is pressed and player is not grounded
        if (Input.GetKeyDown(playerMovement.jumpKey) && !playerMovement.grounded && jumpCount < maxJumps)
        {
            PerformJump();  // Perform the double jump
            jumpCount++;    // Increment the jump count
        }

        // Reset jump count when grounded
        if (playerMovement.grounded)
        {
            jumpCount = 0;  // Reset jump count when touching the ground
        }
    }

    void PerformJump()
    {
        // Reset Y velocity for a consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply the appropriate force for the jump
        float jumpForce = (jumpCount == 0) ? playerMovement.jumpForce : doubleJumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
