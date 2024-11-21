using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnButton : MonoBehaviour
{
    public GameObject playerPrefab; // The player prefab to load in the next scene
    public Transform spawnLocation; // The transform in the next scene where the player should spawn

    // Function to be called by the UI button
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Before loading, destroy any existing player to avoid duplicates
            Destroy(GameObject.FindGameObjectWithTag("Player"));

            // Load the next scene asynchronously
            SceneManager.LoadScene(nextSceneIndex);

            // After the scene is loaded, instantiate the player prefab at the desired spawn location
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (playerPrefab != null && spawnLocation != null)
                {
                    // Instantiate the player prefab at the spawn location's position and rotation
                    Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
                }
                else
                {
                    Debug.LogWarning("Player prefab or spawn location is not assigned.");
                }
            };
        }
        else
        {
            Debug.LogWarning("No more scenes to load!");
        }
    }
}
