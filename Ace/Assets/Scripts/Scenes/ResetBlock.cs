using UnityEngine;
using UnityEngine.SceneManagement; // Include this for scene management

public class ResetBlock : MonoBehaviour
{
    // This method is called when the player touches the reset block (hazard)
    public void OnTriggerEnter(Collider other)
    {
        // Check if the player touched the reset block
        if (other.CompareTag("Player"))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
