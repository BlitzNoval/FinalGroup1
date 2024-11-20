using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeManager : MonoBehaviour
{
    public AbilityAmount abilityAmountScript; 
    public GameObject notice; 
    public AudioClip successSound; 
    public AudioClip failureSound; 
    public AudioSource audioSource; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpUpgrade"))
        {
            ProcessUpgrade(1, typeof(DoubleJumpUpgraded), typeof(DoubleJump), other.gameObject);
        }
        else if (other.CompareTag("DashUpgrade"))
        {
            ProcessUpgrade(3, typeof(DashUpgrade), typeof(Dash), other.gameObject);
        }
        else if (other.CompareTag("TimeSlowUpgrade"))
        {
            ProcessUpgrade(3, typeof(TimeStopAllUpgrade), typeof(TimeSlow), other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the notice when the player leaves the upgrade collider
        if (notice != null && (other.CompareTag("JumpUpgrade") || other.CompareTag("DashUpgrade") || other.CompareTag("TimeSlowUpgrade")))
        {
            notice.SetActive(false);
        }
    }

    private void ProcessUpgrade(int cost, System.Type enableScript, System.Type disableScript, GameObject upgradeObject)
    {
        if (abilityAmountScript.SpendOrbs(cost))
        {
            // Play success sound
            PlaySound(successSound);

            // Enable and disable the corresponding scripts
            var enableComponent = GetComponent(enableScript) as MonoBehaviour;
            if (enableComponent != null) enableComponent.enabled = true;

            var disableComponent = GetComponent(disableScript) as MonoBehaviour;
            if (disableComponent != null) disableComponent.enabled = false;

            // Destroy the upgrade object
            Destroy(upgradeObject);
        }
        else
        {
            // Play failure sound and display notice
            PlaySound(failureSound);
            if (notice != null) notice.SetActive(true);
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