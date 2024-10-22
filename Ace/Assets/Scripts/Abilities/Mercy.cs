using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercy : MonoBehaviour
{

    Vector3 spawnPoint;
    private int logCount = 0; // Counter for logs
    private int logLimit = 500; // Limit on the number of logs
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    // Start is called before the first frame update
    void Start()
    {
        // Get the PlayerMovement component from the player object
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Log player position only if grounded and the log limit has not been reached
        if (playerMovement.grounded && logCount < logLimit)
        {
            print("Player position (Grounded): " + transform.position);
            logCount++; // Increment the log counter
        }
    }

    void onCollisionEnter()
    {

    }

}
