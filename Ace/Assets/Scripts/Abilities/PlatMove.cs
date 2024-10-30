using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatMove : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float distance = 10f; // Distance to move before reversing direction

    private Vector3 startPosition;
    private int direction = 1; // 1 = right, -1 = left

    void Start()
    {
        // Record the starting position of the platform
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the platform in the current direction
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        // Check if the platform has moved the desired distance
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            // Reverse direction
            direction *= -1;

            // Clamp the platform back to the edge of its movement range
            Vector3 clampedPosition = startPosition + Vector3.right * distance * direction;
            transform.position = clampedPosition;
        }
    }
}
