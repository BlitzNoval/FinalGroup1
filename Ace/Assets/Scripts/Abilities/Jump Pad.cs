using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Tooltip("The force applied to objects that touch the jump pad")]
    public float jumpForce = 10f;

    [Tooltip("The direction of the jump force")]
    public Vector3 jumpDirection = Vector3.up;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a Rigidbody
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply the jump force to the Rigidbody
            rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
        }
    }
}