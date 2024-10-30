using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpUpgraded : MonoBehaviour
{
    public int maxJumps = 2;  // the maximum jump count 
    private int jumpCount = 0;  // tracks the number of jumps
    public float doubleJumpForce = 13f;  
    public float superJumpForce = 20f;  // super jump force
    public float chargeTime = 4f;  // the time required to fully charge the super jump
    public float chargeRetentionTime = 10f;  // the duration to retain the full charge of the super jump
    private float chargeTimer = 0f;  // charge time for the super jump
    private float chargeRetentionTimer = 0f;  
    private bool isCharging = false;  
    private bool canSuperJump = false;  
    private bool hasSuperJumped = false;  

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {

        HandleSuperJumpCharging();


        if (Input.GetKeyDown(playerMovement.jumpKey))
        {
            // the player can perform the super jump if fully charged and has not super jumped as yet
            if (canSuperJump && !hasSuperJumped)
            {
                PerformSuperJump();
                hasSuperJumped = true;  
                ResetJumpingAndCharging();
            }
         
            else if (jumpCount < maxJumps && !playerMovement.wallrunning && !hasSuperJumped)
            {
                PerformJump();
                jumpCount++;
            }
        }

        // reset the jumps if the player is grounded
        if (playerMovement.grounded && jumpCount > 0)
        {
            jumpCount = 0;
            hasSuperJumped = false;  
        }
    }

    void HandleSuperJumpCharging()
    {
       
        if (playerMovement.grounded && jumpCount == 0 && !playerMovement.wallrunning && !canSuperJump)
        {
            if (Input.GetKey(KeyCode.LeftControl))  // begin charging if the left control held down
            {
                isCharging = true;

                // add to the charge time only if its not fully charges as yet
                chargeTimer += Time.deltaTime;

                // calculating the charge percentage and stops of the percentage is = 100
                float chargePercentage = Mathf.Clamp01(chargeTimer / chargeTime) * 100f;
                Debug.Log("Charging super jump... " + chargePercentage.ToString("F1") + "%");

                // the super jump is fully charged and the retention timer starts
                if (chargeTimer >= chargeTime)
                {
                    canSuperJump = true;
                    chargeRetentionTimer = chargeRetentionTime;
                    Debug.Log("Super jump fully charged!");
                }
            }
            else if (isCharging)  // resets the charge if its interrupted before full charging
            {
                Debug.Log("Charging interrupted.");
                ResetJumpingAndCharging();
            }
        }

        // manages the charge retention countdown when fully charged
        if (canSuperJump && chargeRetentionTimer > 0)
        {
            chargeRetentionTimer -= Time.deltaTime;

            if (chargeRetentionTimer <= 0)  
            {
                Debug.Log("Charge expired.");
                ResetJumpingAndCharging();
            }
        }
        else if (!playerMovement.grounded)  
        {
            ResetJumpingAndCharging();
        }
    }

    void PerformJump()
    {
        // resets the Y velocity for consistent jumps
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply the jump force
        float jumpForce = (jumpCount == 0) ? playerMovement.jumpForce : doubleJumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void PerformSuperJump()
    {
        // reset the Y velocity for consistent super jump 
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply the super jump force
        rb.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);

        // stops the player from jumping after super jumping until grounded
        jumpCount = maxJumps;
    }

    void ResetJumpingAndCharging()
    {
        isCharging = false;
        chargeTimer = 0f;
        chargeRetentionTimer = 0f;
        canSuperJump = false;
    }
}