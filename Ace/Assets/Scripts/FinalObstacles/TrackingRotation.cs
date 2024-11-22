using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrackRotationPath : MonoBehaviour
{
    [Header("Settings")]
    public Transform trackedPoint; // The point to track on the rotating platform.
    public int maxPoints = 100;    // Maximum number of points in the path.
    public float updateFrequency = 0.1f; // Time interval to sample positions.

    private LineRenderer lineRenderer;
    private float timer = 0f;

    // List to store the tracked positions
    private Vector3[] positions;
    private int currentIndex = 0;

    void Start()
    {
        // Get or initialize the LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            Debug.LogError("LineRenderer component not found!");
            return;
        }

        // Initialize the positions array
        positions = new Vector3[maxPoints];
        for (int i = 0; i < maxPoints; i++)
        {
            positions[i] = trackedPoint ? trackedPoint.position : Vector3.zero;
        }

        // Setup LineRenderer
        lineRenderer.positionCount = maxPoints;
        lineRenderer.loop = false;

        // Set LineRenderer appearance
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 0.5f;
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Sample the position at the given frequency
        if (timer >= updateFrequency)
        {
            timer = 0f;
            TrackPosition();
        }

        // Update the LineRenderer
        UpdateLineRenderer();
    }

    private void TrackPosition()
    {
        if (!trackedPoint) return;

        // Record the new position
        positions[currentIndex] = trackedPoint.position;

        // Increment the index and wrap around if necessary
        currentIndex = (currentIndex + 1) % maxPoints;
    }

    private void UpdateLineRenderer()
    {
        // Update the LineRenderer positions
        int offset = (currentIndex + maxPoints - 1) % maxPoints;
        for (int i = 0; i < maxPoints; i++)
        {
            int index = (offset - i + maxPoints) % maxPoints;
            lineRenderer.SetPosition(i, positions[index]);
        }
    }
}
