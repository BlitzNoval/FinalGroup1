using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Change to a specific scene by name
    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Reload the current scene to fully reset everything and resume time
    public void ResetScene()
    {
        // Resume game time to normal
        Time.timeScale = 1f;

        // Get the active scene and reload it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quit the game application
    public void QuitGame()
    {
        Application.Quit();
    }
}
