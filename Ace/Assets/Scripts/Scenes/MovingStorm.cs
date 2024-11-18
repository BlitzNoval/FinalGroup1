using UnityEngine;

public class MovingStorm : MonoBehaviour
{
    public Transform pointA;  // Set the starting point in the Inspector
    public Transform pointB;  // Set the ending point in the Inspector
    public float speed = 1f;  // Speed at which the storm moves
    public float stopThreshold = 0.1f;  // Small distance to detect when to stop and reverse direction

    private bool movingToB = true;  // Tracks direction: true = moving to pointB, false = moving to pointA

    void Update()
    {
        MoveStorm();
    }

    void MoveStorm()
    {
        if (movingToB)
        {
            // Move from pointA to pointB
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

            // Check if the storm has reached pointB using a small threshold to avoid overshooting
            if (Vector3.Distance(transform.position, pointB.position) < stopThreshold)
            {
                movingToB = false;  // Reverse direction, move back to pointA
            }
        }
        else
        {
            // Move from pointB back to pointA
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

            // Check if the storm has reached pointA using the same threshold
            if (Vector3.Distance(transform.position, pointA.position) < stopThreshold)
            {
                movingToB = true;  // Reverse direction, move back to pointB
            }
        }
    }
}
