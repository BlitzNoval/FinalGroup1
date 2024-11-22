using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [Header("Assign the main particle system head")]
    public ParticleSystem headParticleSystem; // The main particle system head.

    void Start()
    {
        if (headParticleSystem == null)
        {
            Debug.LogError("No head particle system assigned! Please assign one in the Inspector.");
            return;
        }

        // Play the head particle system
        headParticleSystem.Play();

        // Play all child particle systems
        ParticleSystem[] childParticleSystems = headParticleSystem.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in childParticleSystems)
        {
            ps.Play();
        }
    }
}
