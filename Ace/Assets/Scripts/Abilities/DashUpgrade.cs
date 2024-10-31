using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUpgrade : MonoBehaviour
{
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private int maxStamina = 3; // maximum number of dashes 
    [SerializeField] private float staminaRechargeTime = 2f; // the time to recharge each stamina bar

    public KeyCode dashKey = KeyCode.LeftShift;

    private int currentStamina; // the amount of current dashes available
    private bool isRecharging = false; // check if recharging is active
    private Camera mainCamera;
    private Rigidbody rb;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this GameObject");
        }

        currentStamina = maxStamina; 
    }

    private void Update()
    {
        // checks if there is stamina available as well as the dash key is pressed
        if (currentStamina > 0 && Input.GetKeyDown(dashKey))
        {
            PerformDash();
        }
    }

    private void PerformDash()
    {
        currentStamina--; // consumes one stamina point

        // this gets the camera's forward direction for the "omni-directional" dash
        Vector3 dashDirection = mainCamera.transform.forward;
        dashDirection.Normalize();

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        //this will start recharging the stamina if it is below max amount and recharge is not already active
        if (currentStamina < maxStamina && !isRecharging)
        {
            StartCoroutine(RechargeStamina());
        }
    }

    private IEnumerator RechargeStamina()
    {
        isRecharging = true;

        while (currentStamina < maxStamina)
        {
            yield return new WaitForSeconds(staminaRechargeTime);
            currentStamina++; // this will regenerate the stamina points
        }

        isRecharging = false; // resets the recharge status when the stamina fully charged
    }
}
