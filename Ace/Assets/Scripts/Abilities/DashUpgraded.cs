using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUpgraded : MonoBehaviour
{
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private int maxStamina = 3;
    [SerializeField] private float staminaRechargeTime = 2f;
    [SerializeField] private float verticalDashThreshold = 45f; // Angle threshold for vertical dash

    public KeyCode dashKey = KeyCode.LeftShift;

    private int currentStamina;
    private bool isRecharging = false;
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
        if (currentStamina > 0 && Input.GetKeyDown(dashKey))
        {
            PerformDash();
        }
    }

    private void PerformDash()
    {
        currentStamina--;

        Vector3 dashDirection = DetermineDashDirection();
        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        if (currentStamina < maxStamina && !isRecharging)
        {
            StartCoroutine(RechargeStamina());
        }
    }

    private Vector3 DetermineDashDirection()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        float angleWithUp = Vector3.Angle(cameraForward, Vector3.up);
        float angleWithRight = Vector3.Angle(cameraForward, Vector3.right);

        if (angleWithUp <= verticalDashThreshold)
        {
            return Vector3.up;
        }
        else if (angleWithRight <= verticalDashThreshold)
        {
            return Vector3.right * Mathf.Sign(cameraForward.x); // Horizontal dash
        }
        else
        {
            return Vector3.forward * Mathf.Sign(cameraForward.z); // Forward dash
        }
    }

    private IEnumerator RechargeStamina()
    {
        isRecharging = true;

        while (currentStamina < maxStamina)
        {
            yield return new WaitForSeconds(staminaRechargeTime);
            currentStamina++;
        }

        isRecharging = false;
    }
}
