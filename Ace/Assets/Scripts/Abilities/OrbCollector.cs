using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollector : MonoBehaviour
{
    public AbilityAmount abilityAmountScript; 
    public AudioClip collectSound;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            abilityAmountScript.AddOrbs(1);
            PlaySound(collectSound);
            Destroy(other.gameObject);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
