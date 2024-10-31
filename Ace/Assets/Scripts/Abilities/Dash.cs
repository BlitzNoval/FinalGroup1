using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashDistance = 100f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private int maxDashCount = 3; // the number of maximum dashes
    [SerializeField] private float dashRechargeTime = 5f; // time to fully recharge dashes

    private bool readyToDash = true; //  checks if the dash is ready
    private int currentDashCount; // check on the amount of dashes available

    public KeyCode dashKey = KeyCode.LeftShift;

    private PlayerMovement playerMovement;
    private Camera mainCamera;
    private Rigidbody rb;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this GameObject");
        }

        currentDashCount = maxDashCount; 
    }

    private void Update()
    {
        // Check if dash is ready, dashes are available, not wallrunning, and dash key is pressed
        if (readyToDash && currentDashCount > 0 && !playerMovement.wallrunning && Input.GetKeyDown(dashKey))
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        readyToDash = false; //dash is set to being not ready
        currentDashCount--; // decrease the count for the dash

        
        Vector3 dashDirection = mainCamera.transform.forward;
        dashDirection.y = 0; // keeping the direction of the dash horizontal
        dashDirection.Normalize();

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        
        yield return new WaitForSeconds(dashCooldown);
        readyToDash = true; // reset the dash 

        
        if (currentDashCount <= 0)
        {
            StartCoroutine(RechargeDashes());
        }
    }

    private IEnumerator RechargeDashes()
    {
        yield return new WaitForSeconds(dashRechargeTime);
        currentDashCount = maxDashCount; // reset the dash count to max
    }
}

