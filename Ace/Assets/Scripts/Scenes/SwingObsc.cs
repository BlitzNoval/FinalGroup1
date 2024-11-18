using UnityEngine;

public class PendulumObstacle : MonoBehaviour
{
    public float swingSpeed = 2f;  // Speed of pendulum swing
    public float maxSwingAngle = 45f;  // Max swing angle in degrees
    public float knockbackForce = 10f;  // Force to knock back the player

    private Quaternion startRotation;
    private Rigidbody rb;  // Rigidbody for pendulum physics

    void Start()
    {
        startRotation = transform.rotation;  // Store the starting rotation
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  // We don't want the pendulum to move due to physics
    }

    void Update()
    {
        SwingPendulum();
    }

    void SwingPendulum()
    {
        // Swing back and forth between -maxSwingAngle and maxSwingAngle using a sine wave
        float angle = Mathf.Sin(Time.time * swingSpeed) * maxSwingAngle;
        transform.rotation = startRotation * Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply knockback force to the player
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                Debug.Log("Player hit by pendulum and knocked back!");
            }
        }
    }
}
