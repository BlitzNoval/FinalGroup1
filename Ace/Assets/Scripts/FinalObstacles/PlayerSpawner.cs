using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // The player prefab to spawn
    public Transform spawnPoint;   // The specific point to spawn the player

    private void Start()
    {
        if (playerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Player prefab or spawn point is not assigned!");
            return;
        }

        // Check if a player instance already exists in the scene
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Debug.Log("Player already exists in the scene.");
        }
        else
        {
            // Instantiate the player prefab at the spawn point
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Player spawned at the specified point.");
        }
    }
}
