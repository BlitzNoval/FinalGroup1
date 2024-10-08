using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetOnTouch : MonoBehaviour
{
    // This function is called when the object with this script collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reload the current scene (reset)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Optional: If you want the object to reset the scene on trigger (non-physical collision)
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Reload the current scene (reset)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
