using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Tooltip("The force applied to objects that touch the jump pad")]
    public float jumpForce = 10f;

    [Tooltip("The direction of the jump force")]
    public Vector3 jumpDirection = Vector3.up;

    [Tooltip("Particle system for the idle glow effect")]
    public ParticleSystem idleGlowEffect;

    [Tooltip("Particle system for the bounce effect")]
    public ParticleSystem bounceEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a Rigidbody
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply the jump force to the Rigidbody
            rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);

            // Trigger the bounce effect
            TriggerBounceEffect(collision.GetContact(0).point);
        }
    }

    private void TriggerBounceEffect(Vector3 position)
    {
        if (bounceEffect != null)
        {
            ParticleSystem instance = Instantiate(bounceEffect, position, Quaternion.identity);
            instance.transform.up = jumpDirection; // Align particles with jump direction
            instance.Play();
            Destroy(instance.gameObject, instance.main.duration);
        }
    }

    private void Start()
    {
        // Play idle glow effect if assigned
        if (idleGlowEffect != null)
        {
            idleGlowEffect.Play();
        }
    }
}
