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

    // UI Elements
    public GameObject jumpUpgradeUI;          // UI for the Jump Upgrade
    public GameObject doubleJumpUI;           // UI for the upgraded Double Jump
    public GameObject dashUpgradeUI;          // UI for the Dash Upgrade
    public GameObject dashUpgradedUI;         // UI for the upgraded Dash
    public GameObject timeSlowUpgradeUI;      // UI for the Time Slow Upgrade
    public GameObject timeStopAllUpgradeUI;   // UI for the upgraded Time Slow

    
    void Start()
    {

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpUpgrade"))
        {
            ProcessUpgrade(1, typeof(DoubleJumpUpgraded), typeof(DoubleJump), other.gameObject, jumpUpgradeUI, doubleJumpUI);
        }
        else if (other.CompareTag("DashUpgrade"))
        {
            ProcessUpgrade(3, typeof(DashUpgrade), typeof(Dash), other.gameObject, dashUpgradeUI, dashUpgradedUI);
        }
        else if (other.CompareTag("TimeSlowUpgrade"))
        {
            ProcessUpgrade(3, typeof(TimeStopAllUpgrade), typeof(TimeSlow), other.gameObject, timeSlowUpgradeUI, timeStopAllUpgradeUI);
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

    private void ProcessUpgrade(int cost, System.Type enableScript, System.Type disableScript, GameObject upgradeObject, GameObject currentUI, GameObject upgradedUI)
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

            // Enable the upgraded UI and disable the previous UI
            if (currentUI != null) currentUI.SetActive(false);
            if (upgradedUI != null) upgradedUI.SetActive(true);

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