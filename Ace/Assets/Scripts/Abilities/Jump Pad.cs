using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public Vector3 jumpDirection = Vector3.up;

    [Header("Sound Settings")]
    public AudioClip bounceSound;
    [Range(0f, 1f)] public float soundVolume = 0.7f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.5f; // 3D sound with a more natural stereo blend
        audioSource.maxDistance = 1f; // Optional: Set maximum distance for sound to be heard
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Optional: Linear rolloff for a smooth volume change over distance
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply jump force
            rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);

            // Play sound
            if (bounceSound != null)
            {
                audioSource.PlayOneShot(bounceSound, soundVolume);
            }
        }
    }
}
