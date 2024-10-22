using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointAndResetSystem : MonoBehaviour
{
    [SerializeField] private Transform currentCheckpoint;
    public GameObject player;
    
    // New variable for respawn height offset
    [SerializeField] private float respawnHeightOffset = 1f; // Adjust this value as needed

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ResetOrRespawnPlayer();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetOrRespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        if (currentCheckpoint != null)
        {
            // Calculate the respawn position with the height offset
            Vector3 respawnPosition = currentCheckpoint.position + Vector3.up * respawnHeightOffset;
            
            // Move the player to the calculated respawn position
            player.transform.position = respawnPosition;
            
            Debug.Log($"Player respawned at checkpoint: {currentCheckpoint.name} with height offset");
            
            // Optionally, you might want to reset the player's velocity if they have a Rigidbody
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Unable to respawn player.");
        }
    }

    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint updated to: " + newCheckpoint.name);
    }

    public bool HasCheckpoint()
    {
        return currentCheckpoint != null;
    }

    private void ResetOrRespawnPlayer()
    {
        if (HasCheckpoint())
        {
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}