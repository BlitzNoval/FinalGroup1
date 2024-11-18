using UnityEngine;

public class TrapWithGracePeriod : MonoBehaviour
{
    public float gracePeriod = 2f;  // Time before the player is destroyed
    private bool playerInTrap = false;  // Track if the player is in the trap
    private float timer = 0f;  // Timer to count the grace period

    void Update()
    {
        if (playerInTrap)
        {
            timer += Time.deltaTime;  // Start counting the time

            if (timer >= gracePeriod)
            {
                DestroyPlayer();  // Destroy the player after the grace period
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrap = true;  // Start the timer when the player enters the trap
            timer = 0f;  // Reset timer
            Debug.Log("Player entered trap! Move out quickly!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrap = false;  // Stop the timer when the player exits the trap
            timer = 0f;  // Reset timer
            Debug.Log("Player escaped the trap!");
        }
    }

    void DestroyPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);  // Destroy the player
            Debug.Log("Player was caught in the trap and destroyed!");
        }
    }
}
