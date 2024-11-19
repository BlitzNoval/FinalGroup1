using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnTouch : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Alternative: Check if the object that triggered has the "Player" tag
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes to load!");
        }
    }
}
