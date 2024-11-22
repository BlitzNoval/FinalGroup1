using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProceduralGlassShatter : MonoBehaviour
{
    [Header("Shatter Settings")]
    [SerializeField] private int fragmentCount = 15;
    [SerializeField] private float shatterForce = 10f;
    [SerializeField] private float fragmentScale = 0.98f;
    [SerializeField] private Material glassMaterial;
    [SerializeField] private float triggerThreshold = 5f; // Minimum velocity needed to break glass
    
    [Header("Audio")]
    [SerializeField] private AudioClip shatterSound;
    
    private bool hasShattered = false;
    private Mesh originalMesh;
    private List<Vector3> shatterPoints;
    
    private void Start()
    {
        // Cache the original mesh
        originalMesh = GetComponent<MeshFilter>().mesh;
        glassMaterial = GetComponent<MeshRenderer>().material;
        
        // Replace MeshCollider with a trigger collider
        Destroy(GetComponent<MeshCollider>());
        BoxCollider trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        
        // Make the glass material not interact with physics
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasShattered)
        {
            // Calculate impact velocity
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            float impactVelocity = playerRb != null ? playerRb.velocity.magnitude : 0f;
            
            // If player is moving fast enough or we don't care about velocity
            if (impactVelocity >= triggerThreshold || triggerThreshold <= 0)
            {
                // Get the closest point on the glass to the player
                Vector3 impactPoint = transform.position;
                if (other is CapsuleCollider capsule)
                {
                    // For character controllers/capsule colliders
                    impactPoint = Physics.ClosestPoint(other.transform.position, GetComponent<Collider>(), transform.position, transform.rotation);
                }
                else
                {
                    // For other collider types
                    impactPoint = other.ClosestPoint(transform.position);
                }
                
                ShatterAtPoint(impactPoint);
            }
        }
    }
   // Rest of the methods remain the same
    private void ShatterAtPoint(Vector3 impactPoint)
    {
        hasShattered = true;
        
        // Generate random points for Voronoi cells
        GenerateShatterPoints(impactPoint);
        
        // Create fragments
        List<GlassFragment> fragments = CreateFragments();
        
        // Apply physics forces to fragments
        ApplyShatterForces(fragments, impactPoint);
        
        // Play sound
        if (shatterSound != null)
        {
            AudioSource.PlayClipAtPoint(shatterSound, transform.position);
        }
        
        // Disable original object immediately
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        
        // Delay the full deactivation slightly to ensure sound plays
        Invoke("DisableObject", 0.1f);
    }
    
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
    
    
    private void GenerateShatterPoints(Vector3 impactPoint)
    {
        shatterPoints = new List<Vector3>();
        
        // Convert impact point to local space
        Vector3 localImpact = transform.InverseTransformPoint(impactPoint);
        
        // Add impact point and points around it
        shatterPoints.Add(localImpact);
        
        for (int i = 0; i < fragmentCount - 1; i++)
        {
            // Generate points with higher density near impact
            float distance = Random.Range(0f, 1f);
            distance = Mathf.Pow(distance, 2); // Square for more points near impact
            
            Vector3 randomDir = Random.onUnitSphere;
            Vector3 point = localImpact + randomDir * distance;
            
            // Project point onto mesh bounds
            Bounds bounds = originalMesh.bounds;
            point = new Vector3(
                Mathf.Clamp(point.x, bounds.min.x, bounds.max.x),
                Mathf.Clamp(point.y, bounds.min.y, bounds.max.y),
                Mathf.Clamp(point.z, bounds.min.z, bounds.max.z)
            );
            
            shatterPoints.Add(point);
        }
    }
    
    private List<GlassFragment> CreateFragments()
    {
        List<GlassFragment> fragments = new List<GlassFragment>();
        
        // Create fragments for each Voronoi cell
        for (int i = 0; i < shatterPoints.Count; i++)
        {
            GameObject fragmentObj = new GameObject($"Fragment_{i}");
            fragmentObj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            fragmentObj.transform.localScale = transform.localScale * fragmentScale;
            
            // Add components
            MeshFilter meshFilter = fragmentObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = fragmentObj.AddComponent<MeshRenderer>();
            MeshCollider meshCollider = fragmentObj.AddComponent<MeshCollider>();
            Rigidbody rb = fragmentObj.AddComponent<Rigidbody>();
            
            // Create mesh for this fragment
            Mesh fragmentMesh = CreateFragmentMesh(shatterPoints[i], shatterPoints);
            meshFilter.mesh = fragmentMesh;
            meshCollider.sharedMesh = fragmentMesh;
            meshRenderer.material = glassMaterial;
            
            // Configure physics
            rb.mass = fragmentMesh.bounds.size.magnitude;
            rb.drag = 0.5f;
            rb.angularDrag = 0.5f;
            
            // Add to list
            fragments.Add(new GlassFragment
            {
                GameObject = fragmentObj,
                Rigidbody = rb,
                CenterPoint = shatterPoints[i]
            });
        }
        
        return fragments;
    }
    
    private Mesh CreateFragmentMesh(Vector3 centerPoint, List<Vector3> otherPoints)
    {
        Mesh fragmentMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        // Get original mesh data
        Vector3[] originalVertices = originalMesh.vertices;
        int[] originalTriangles = originalMesh.triangles;
        
        // For each triangle in original mesh
        for (int i = 0; i < originalTriangles.Length; i += 3)
        {
            Vector3 v1 = originalVertices[originalTriangles[i]];
            Vector3 v2 = originalVertices[originalTriangles[i + 1]];
            Vector3 v3 = originalVertices[originalTriangles[i + 2]];
            
            // Calculate triangle centroid
            Vector3 centroid = (v1 + v2 + v3) / 3f;
            
            // If this triangle is closest to our center point compared to other points
            if (IsClosestPoint(centroid, centerPoint, otherPoints))
            {
                // Add triangle to fragment mesh
                int baseIndex = vertices.Count;
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                triangles.Add(baseIndex);
                triangles.Add(baseIndex + 1);
                triangles.Add(baseIndex + 2);
            }
        }
        
        // If no triangles were added, create a small triangle to prevent empty mesh
        if (triangles.Count == 0)
        {
            Vector3 small = Vector3.one * 0.01f;
            vertices.Add(centerPoint);
            vertices.Add(centerPoint + small);
            vertices.Add(centerPoint - small);
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
        }
        
        fragmentMesh.vertices = vertices.ToArray();
        fragmentMesh.triangles = triangles.ToArray();
        fragmentMesh.RecalculateNormals();
        fragmentMesh.RecalculateBounds();
        
        return fragmentMesh;
    }
    
    private bool IsClosestPoint(Vector3 position, Vector3 target, List<Vector3> points)
    {
        float targetDistance = Vector3.Distance(position, target);
        return points.All(p => Vector3.Distance(position, p) >= targetDistance);
    }
    
    private void ApplyShatterForces(List<GlassFragment> fragments, Vector3 impactPoint)
    {
        foreach (var fragment in fragments)
        {
            Vector3 fragmentCenter = fragment.GameObject.transform.TransformPoint(fragment.CenterPoint);
            Vector3 awayFromImpact = (fragmentCenter - impactPoint).normalized;
            
            float distanceFactor = 1f - Mathf.Clamp01(Vector3.Distance(fragmentCenter, impactPoint) / 2f);
            float fragmentForce = shatterForce * distanceFactor;
            
            fragment.Rigidbody.AddForce(awayFromImpact * fragmentForce, ForceMode.Impulse);
            fragment.Rigidbody.AddTorque(Random.insideUnitSphere * fragmentForce, ForceMode.Impulse);
            
            // Destroy fragments after delay
            Destroy(fragment.GameObject, 5f);
        }
    }
    
    private class GlassFragment
    {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Vector3 CenterPoint;
    }
}