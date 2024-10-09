using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 100f; 
    public float dashSpeed = 50f; 
    public float dashCooldown = 1f; 
    private bool readyToDash = true; // Check if the dash is ready

    public KeyCode dashKey = KeyCode.LeftShift; 

    private PlayerMovement playerMovement; 
    private Camera mainCamera;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mainCamera = Camera.main; 
    }

    private void Update()
    {
        // Check if dash is ready, not wallrunning, and dash key is pressed
        if (readyToDash && !playerMovement.wallrunning && Input.GetKeyDown(dashKey))
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        readyToDash = false; // Set dash to not ready

        // Get the camera's forward direction and perform the dash
        Vector3 dashDirection = mainCamera.transform.forward; // Dash in the direction the camera is facing
        dashDirection.y = 0; // Keep the dash direction horizontal
        dashDirection.Normalize(); // Normalize the direction vector

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse); // Use dashSpeed for the force applied

        // Wait for the dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        readyToDash = true; // Reset dash readiness
    }
}


