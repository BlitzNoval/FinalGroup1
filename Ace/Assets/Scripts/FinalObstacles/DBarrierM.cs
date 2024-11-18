using UnityEngine;

public class MovableDeathBarrier : MonoBehaviour
{
    public Vector3 movementDirection = Vector3.right; // Direction of movement
    public float speed = 2f; // Speed of movement
    public float movementDistance = 5f; // Total distance to move back and forth

    private Vector3 startPosition;
    private bool movingForward = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, startPosition);

        if (movingForward && distance >= movementDistance)
        {
            movingForward = false;
        }
        else if (!movingForward && distance <= 0f)
        {
            movingForward = true;
        }

        Vector3 direction = movingForward ? movementDirection : -movementDirection;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
