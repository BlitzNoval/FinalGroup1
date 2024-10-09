using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign the projectile prefab in the Inspector
    public Transform launchPoint;        // Assign the launch point transform in the Inspector
    public float launchForce = 10f;      // Speed of the projectile
    public float fireRate = 1f;          // Time between shots

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

        // Apply force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(launchPoint.forward * launchForce, ForceMode.Impulse);
        }
    }
}
