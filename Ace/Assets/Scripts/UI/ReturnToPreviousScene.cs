using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToPreviousScene : MonoBehaviour
{
    public int sceneIndex; // Set this in the Inspector or dynamically via code to specify which scene to load
    public GameObject playerPrefab; // Reference to the player prefab
    public Transform newSpawnLocation; // A new spawn location for the player

    private GameObject player; // Reference to the instantiated player
    private float timeBetweenClicks = 0.5f; // Time window for double-click (in seconds)
    private float lastClickTime = 0f; // Time of the last click
    private bool isReadyForDoubleClick = false; // Whether the system is ready to detect the second click

    // Resumes the game from the previous scene (after going to pause menu)
    public void ResumeFromPause()
    {
        // Unload the pause menu scene if needed
        SceneManager.UnloadSceneAsync(sceneIndex);

        // Resume the game time
        Time.timeScale = 1;

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Load the specified scene
        SceneManager.LoadScene(sceneIndex);
    }

    // Restarts the game, but only if there are two clicks within the time window
    public void RestartGame()
    {
        // If we're within the time window for a double click, process the restart
        if (isReadyForDoubleClick && Time.time - lastClickTime <= timeBetweenClicks)
        {
            // Set time scale to normal before restarting
            Time.timeScale = 1;

            // Lock and hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Get the current scene and reload it
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Load the scene to reset it
            SceneManager.LoadScene(currentSceneName);

            // Start a coroutine to instantiate and place the player at the new spawn location
            StartCoroutine(PlacePlayerAtNewLocation());
        }
        else
        {
            // Record the first click and set the flag to detect the second click
            isReadyForDoubleClick = true;
            lastClickTime = Time.time;

            // Optional: Show a UI message indicating that the user needs to click again for restart
            Debug.Log("Click again to confirm restart.");
        }
    }

    // Coroutine to instantiate the player and place them at the new spawn location after the scene reloads
    private IEnumerator PlacePlayerAtNewLocation()
    {
        // Wait until the next frame to ensure the scene is fully reloaded
        yield return new WaitForEndOfFrame();  // This ensures the scene has completed reloading

        // If the player is not already in the scene, instantiate it
        player = GameObject.FindWithTag("Player"); // Make sure the player is tagged as "Player"

        if (player == null && playerPrefab != null && newSpawnLocation != null)
        {
            // Instantiate the player prefab at the new spawn location
            player = Instantiate(playerPrefab, newSpawnLocation.position, newSpawnLocation.rotation);
        }
        else if (player != null && newSpawnLocation != null)
        {
            // If the player is already present in the scene, move it to the new location
            player.transform.position = newSpawnLocation.position;
            player.transform.rotation = newSpawnLocation.rotation;
        }

        // Reset the double-click detection flag after the restart action is completed
        isReadyForDoubleClick = false;
    }
}