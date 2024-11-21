using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        // Toggle pause state when 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Stop the game time
        isPaused = true;
        SceneManager.LoadScene("pauseScreen", LoadSceneMode.Additive); // Load the pause scene
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game time
        isPaused = false;
        SceneManager.UnloadSceneAsync("pauseScreen"); // Unload the pause scene
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
