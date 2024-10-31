using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;  // Start point
    public Transform pointB;  // End point
    public float speed = 2f;  // Platform speed

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = pointB.position;  // Start by moving towards point B
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        // Move platform between points A and B
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Switch target once it reaches a point
        if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
        }
    }

    // Make sure to move the entire player hierarchy (including camera)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Parent the player to the platform, which will move the camera with the player
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Unparent the player from the platform
            other.transform.SetParent(null);
        }
    }
}
