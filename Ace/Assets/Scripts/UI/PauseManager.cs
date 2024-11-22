using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public int pauseMenuSceneIndex; // The index of the pause menu scene
    private int previousSceneIndex; // The index of the scene before the pause menu
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
        }
    }

    // Pauses the game and loads the pause menu
    public void PauseGame()
    {
        isPaused = true;

        // Store the current scene index
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Pause the game time
        Time.timeScale = 0;

        // Load the pause menu scene
        SceneManager.LoadScene(pauseMenuSceneIndex, LoadSceneMode.Additive);

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
