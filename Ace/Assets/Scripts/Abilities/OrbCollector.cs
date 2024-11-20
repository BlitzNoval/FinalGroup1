using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollector : MonoBehaviour
{
    public AbilityAmount abilityAmountScript;  // Link to AbilityAmount script
    public AudioClip collectSound;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            abilityAmountScript.AddOrbs(1); // Add 1 orb when an orb is collected
            PlaySound(collectSound);
            Destroy(other.gameObject); // Destroy the orb
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Play sound effect
        }
    }

}
