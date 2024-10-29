using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercy : MonoBehaviour
{
    private Vector3 lastGroundedPoint;  // Store last grounded point
    private PlayerMovement playerMovement;  // Reference to PlayerMovement script
    private bool isOnCooldown = false;  // Cooldown state
    public int respawnLimit = 3;  // Number of respawns allowed
    public float respawnCooldown = 5f;  // Cooldown duration
    private int respawnCount = 0;  // Track number of respawns
    public CheckpointAndResetSystem checkpointSystem;  // Reference to the checkpoint system
    private bool mercyActive = false;  // Flag to control respawn priority

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lastGroundedPoint = transform.position;  // Initialize at player's starting position
    }

    // Update is called once per frame
    void Update()
    {
        // Update the last grounded point when the player is grounded
        if (playerMovement.grounded)
        {
            lastGroundedPoint = transform.position;
        }
    }

    // Call this function when the player "dies"
    public void OnPlayerDeath()
    {
        if (CanRespawn())
        {
            // If Mercy system can handle the respawn, trigger it
            RespawnAtLastGroundedPoint();
        }
        else
        {
            // Otherwise, allow the checkpoint system to handle the respawn
            mercyActive = false;
            checkpointSystem.ResetOrRespawnPlayer();
        }
    }

    // Method to check if the Mercy system can handle the respawn
    public bool CanRespawn()
    {
        if (respawnCount < respawnLimit && !isOnCooldown)
        {
            mercyActive = true;
            return true; // Mercy system will handle the respawn
        }
        return false; // Mercy system cannot handle
    }

    // Respawn the player at the last grounded point
    private void RespawnAtLastGroundedPoint()
    {
        transform.position = lastGroundedPoint;
        respawnCount++;
        Debug.Log("Player respawned at last grounded point: " + lastGroundedPoint);

        // Start cooldown after respawn
        StartCoroutine(RespawnCooldown());
    }

    // Cooldown coroutine
    private IEnumerator RespawnCooldown()
    {
        isOnCooldown = true;
        Debug.Log("Mercy system cooldown started.");
        yield return new WaitForSeconds(respawnCooldown);
        isOnCooldown = false;
        Debug.Log("Mercy system cooldown ended.");
    }

    // Public method to check if Mercy is active and in control
    public bool IsMercyActive()
    {
        return mercyActive;
    }
}
