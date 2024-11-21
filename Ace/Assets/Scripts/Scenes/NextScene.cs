using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene: MonoBehaviour
{
    public GameObject playerPrefab; // The player prefab to load in the next scene
    public Transform spawnLocation; // The transform in the next scene where the player should spawn
    public int nextSceneIndex; // The index of the next scene to load

    // Method to be used in the Button OnClick() event
    public void OnLoadButtonClick()
    {
        // Ensure that the player is not already instantiated
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer); // Destroy the old player to avoid duplicates
        }

        // Before loading, check if the next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene asynchronously
            SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Single).completed += (operation) =>
            {
                // After the scene has loaded, instantiate the player prefab at the desired spawn location
                if (playerPrefab != null && spawnLocation != null)
                {
                    GameObject player = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

                    // Check if the player prefab has the singleton component, and set it
                    PlayerSingleton playerSingleton = player.GetComponent<PlayerSingleton>();
                    if (playerSingleton != null)
                    {
                        playerSingleton.SetInstance(player); // Set the singleton instance if it exists
                    }

                    Debug.Log("Player successfully loaded into the new scene.");

                    // Set the timescale to 1 (normal speed)
                    Time.timeScale = 1;
                }
                else
                {
                    Debug.LogError("Player prefab or spawn location is not assigned.");
                }
            };
        }
        else
        {
            Debug.LogError("Next scene index is out of range.");
        }
    }
}
