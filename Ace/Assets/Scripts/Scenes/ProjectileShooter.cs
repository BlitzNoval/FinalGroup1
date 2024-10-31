using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign the projectile prefab in the Inspector
    public Transform launchPoint;        // Assign the launch point transform in the Inspector
    public float launchForce = 1000f;    // Increased launch force
    public float fireRate = 1f;          // Time between shots
    public CheckpointAndResetSystem checkpointSystem; // Reference to the CheckpointAndResetSystem

    private float nextFireTime = 0f;

    void Update()
    {
        // Launch projectile based on fire rate timing
        if (Time.time > nextFireTime)
        {
            LaunchProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void LaunchProjectile()
    {
        // Instantiate the projectile at the launch point
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);

        // Get the Projectile component and set the checkpoint system
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetCheckpointSystem(checkpointSystem);
        }

        // Apply force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Ensure the rigidbody is not kinematic
            rb.isKinematic = false;
            
            // Reset the velocity before applying force
            rb.velocity = Vector3.zero;
            
            // Apply the force in the forward direction of the launch point
            rb.AddForce(launchPoint.forward * launchForce, ForceMode.Impulse);
            
            // Optional: If you want to disable gravity
            // rb.useGravity = false;
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody component!");
        }
    }
}