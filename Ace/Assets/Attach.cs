using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Set the Player's root GameObject as a child of the moving platform
            other.transform.root.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Detach the Player's root GameObject from the moving platform
            other.transform.root.SetParent(null);
        }
    }
}
