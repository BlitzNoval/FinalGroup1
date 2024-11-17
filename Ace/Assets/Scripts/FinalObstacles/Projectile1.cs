using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    public float knockbackForce = 10f; // Knockback force applied to the player

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit has a Rigidbody
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Apply knockback
            Vector3 knockbackDirection = collision.transform.position - transform.position;
            knockbackDirection.y = 0; // Optional: Keep knockback horizontal
            knockbackDirection.Normalize();

            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

        // Destroy the projectile
        Destroy(gameObject);
    }
}
