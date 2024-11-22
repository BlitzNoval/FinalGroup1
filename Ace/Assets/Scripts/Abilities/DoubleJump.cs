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

    // Particle effects
    [Header("Double Jump Feedback")]
    public GameObject doubleJumpParticleEffect1; // First particle effect
    public GameObject doubleJumpParticleEffect2; // Second particle effect

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        remainingJumps = extraJumps; // Initialize remaining jumps to the allowed extra jumps

        // Disable both particle effects at the start
        if (doubleJumpParticleEffect1 != null)
            doubleJumpParticleEffect1.SetActive(false);
        if (doubleJumpParticleEffect2 != null)
            doubleJumpParticleEffect2.SetActive(false);
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

        // Enable the particle effects for 1 second
        EnableDoubleJumpParticles();
    }

    private void EnableDoubleJumpParticles()
    {
        // Enable the first particle effect
        if (doubleJumpParticleEffect1 != null)
        {
            doubleJumpParticleEffect1.SetActive(true);
        }

        // Enable the second particle effect
        if (doubleJumpParticleEffect2 != null)
        {
            doubleJumpParticleEffect2.SetActive(true);
        }

        // Disable the particle effects after 1 second
        StartCoroutine(DisableDoubleJumpParticles());
    }

    private IEnumerator DisableDoubleJumpParticles()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0.5f);

        // Disable the particle effects
        if (doubleJumpParticleEffect1 != null)
        {
            doubleJumpParticleEffect1.SetActive(false);
        }

        if (doubleJumpParticleEffect2 != null)
        {
            doubleJumpParticleEffect2.SetActive(false);
        }
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