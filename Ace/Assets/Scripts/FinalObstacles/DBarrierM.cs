using UnityEngine;

public class MovableDeathBarrier : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 movementDirection = Vector3.right;
    public float speed = 2f;
    public float movementDistance = 5f;

    [Header("Optional Behavior")]
    [Tooltip("If true, the object will pause briefly at each end")]
    public bool pauseAtEnds = false;
    public float pauseDuration = 0.5f;

    [Header("Movement Type")]
    [Tooltip("Choose between Linear or Smooth movement")]
    public MovementType movementType = MovementType.Smooth;

    [Header("Rotation Settings")]
    [Tooltip("If true, the barrier will rotate continuously.")]
    public bool enableRotation = false;
    public float rotationSpeed = 100f; // Degrees per second

    [Tooltip("Choose the direction of rotation: Clockwise or Counterclockwise.")]
    public RotationDirection rotationDirection = RotationDirection.Clockwise;

    [Tooltip("Choose the axis of rotation: X, Y, or Z.")]
    public RotationAxis rotationAxis = RotationAxis.Z;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentLerpTime = 0f;
    private float pauseTimer = 0f;
    private bool isPaused = false;

    public enum MovementType
    {
        Linear,
        Smooth
    }

    public enum RotationDirection
    {
        Clockwise,
        Counterclockwise
    }

    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    private void Start()
    {
        startPosition = transform.position;
        // Calculate the end position based on movement direction and distance
        endPosition = startPosition + (movementDirection.normalized * movementDistance);

        // Validate settings
        if (speed <= 0)
        {
            Debug.LogWarning("Speed must be greater than 0. Setting to default value of 2.");
            speed = 2f;
        }

        if (movementDistance <= 0)
        {
            Debug.LogWarning("Movement distance must be greater than 0. Setting to default value of 5.");
            movementDistance = 5f;
        }
    }

    private void Update()
    {
        if (isPaused)
        {
            HandlePause();
            return;
        }

        // Update the lerp time
        currentLerpTime += Time.deltaTime * speed;
        float percentageComplete = currentLerpTime / 2f; // Divide by 2 for complete back and forth cycle

        // Create a ping-pong effect between 0 and 1
        float pingPong = Mathf.PingPong(percentageComplete, 1f);

        // Apply different movement types
        Vector3 newPosition;
        if (movementType == MovementType.Smooth)
        {
            // Use smooth interpolation
            pingPong = Mathf.SmoothStep(0f, 1f, pingPong);
        }

        // Calculate the new position
        newPosition = Vector3.Lerp(startPosition, endPosition, pingPong);
        transform.position = newPosition;

        // Check if we need to pause at the ends
        if (pauseAtEnds)
        {
            if (Vector3.Distance(transform.position, startPosition) < 0.01f ||
                Vector3.Distance(transform.position, endPosition) < 0.01f)
            {
                isPaused = true;
                pauseTimer = pauseDuration;
            }
        }

        // Apply rotation if enabled
        if (enableRotation)
        {
            float rotationDirectionMultiplier = rotationDirection == RotationDirection.Clockwise ? -1f : 1f;
            Vector3 axis = Vector3.zero;

            switch (rotationAxis)
            {
                case RotationAxis.X:
                    axis = Vector3.right;
                    break;
                case RotationAxis.Y:
                    axis = Vector3.up;
                    break;
                case RotationAxis.Z:
                    axis = Vector3.forward;
                    break;
            }

            transform.Rotate(axis, rotationDirectionMultiplier * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void HandlePause()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            isPaused = false;
        }
    }

    // Optional: Visualize the movement path in the editor
    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        Vector3 start = transform.position;
        Vector3 end = start + (movementDirection.normalized * movementDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.2f);
        Gizmos.DrawWireSphere(end, 0.2f);
    }
}
