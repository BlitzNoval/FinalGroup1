using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab
    public Transform firePoint; // The point where the projectile is spawned
    public float fireRate = 2f; // Time between shots
    public float projectileForce = 500f; // Force applied to the projectile

    private float nextFireTime;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            LaunchProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void LaunchProjectile()
    {
        // Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce);
        }

        // Destroy projectile after a certain time to save memory
        Destroy(projectile, 5f);
    }
}
