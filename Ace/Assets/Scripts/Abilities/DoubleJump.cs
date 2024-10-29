using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public int maxJumps = 2;  // allows for both jumps (normal & double jump)
    private int jumpCount = 0;  // tracks the amount of jumps
    public float doubleJumpForce = 13f;  // the force of the jumps

    private Rigidbody rb;
    private PlayerMovement playerMovement;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // checks if the jump key is pressed, player is not grounded, and the player is not wallrunning
        if (Input.GetKeyDown(playerMovement.jumpKey) && !playerMovement.grounded && jumpCount < maxJumps && !playerMovement.wallrunning)
        {
            PerformJump();  
            jumpCount++;   
        }

        // reset the jump count when the player is grounded
        if (playerMovement.grounded)
        {
            jumpCount = 0;  
        }
    }

    void PerformJump()
    {
        // resets the Y velocity for a consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply jump force
        float jumpForce = (jumpCount == 0) ? playerMovement.jumpForce : doubleJumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
