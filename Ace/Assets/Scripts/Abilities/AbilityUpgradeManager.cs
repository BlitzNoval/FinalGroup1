using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeManager : MonoBehaviour
{
    public AbilityAmount abilityAmountScript;
    public GameObject notice;
    public GameObject dashNotice;
    public GameObject jumpNotice;
    public GameObject timeNotice;
    public AudioClip successSound;
    public AudioClip failureSound;
    public AudioSource audioSource;

    // UI Elements
    public GameObject jumpUpgradeUI;
    public GameObject doubleJumpUI;
    public GameObject dashUI;
    public GameObject dashUpgradedUI;
    public GameObject timeSlowUI;
    public GameObject timeStopAllUpgradeUI;

    private GameObject currentUpgrade;
    private string currentTag;
    private int currentCost;
    private System.Type enableScript;
    private System.Type disableScript;
    private GameObject currentUI;
    private GameObject upgradedUI;

    void Start()
    {
        // Ensure that the notices and UI elements persist between scenes
        DontDestroyOnLoad(notice);
        DontDestroyOnLoad(dashNotice);
        DontDestroyOnLoad(jumpNotice);
        DontDestroyOnLoad(timeNotice);

        doubleJumpUI.SetActive(true);
        dashUI.SetActive(true);
        timeSlowUI.SetActive(true);

        jumpUpgradeUI.SetActive(false);
        dashUpgradedUI.SetActive(false);
        timeStopAllUpgradeUI.SetActive(false);

        notice.SetActive(false);
        jumpNotice.SetActive(false);
        dashNotice.SetActive(false);
        timeNotice.SetActive(false);
    }

    private void Update()
    {
        if (currentUpgrade != null && Input.GetKeyDown(KeyCode.E))
        {
            ProcessUpgrade(currentCost, enableScript, disableScript, currentUpgrade, currentUI, upgradedUI);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpUpgrade"))
        {
            ShowNotice(jumpNotice);
            SetUpgradeDetails(1, typeof(DoubleJumpUpgraded), typeof(DoubleJump), other.gameObject, jumpUpgradeUI, doubleJumpUI, "JumpUpgrade");
        }
        else if (other.CompareTag("DashUpgrade"))
        {
            ShowNotice(dashNotice);
            SetUpgradeDetails(3, typeof(DashUpgrade), typeof(Dash), other.gameObject, dashUI, dashUpgradedUI, "DashUpgrade");
        }
        else if (other.CompareTag("TimeSlowUpgrade"))
        {
            ShowNotice(timeNotice);
            SetUpgradeDetails(3, typeof(TimeStopAllUpgrade), typeof(TimeSlow), other.gameObject, timeSlowUI, timeStopAllUpgradeUI, "TimeSlowUpgrade");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the tag is null or empty
        if (string.IsNullOrEmpty(other.tag)) return; // Exit if the tag is not set or empty

        // Hide the notice when the player leaves the upgrade collider
        if (notice != null && (other.CompareTag("JumpUpgrade") || other.CompareTag("DashUpgrade") || other.CompareTag("TimeSlowUpgrade")))
        {
            notice.SetActive(false);
        }
    }

    private void ShowNotice(GameObject noticeObject)
    {
        HideAllNotices();
        if (noticeObject != null)
        {
            noticeObject.SetActive(true);
        }
    }

    private void HideAllNotices()
    {
        notice.SetActive(false);
        jumpNotice.SetActive(false);
        dashNotice.SetActive(false);
        timeNotice.SetActive(false);
    }

    private void SetUpgradeDetails(int cost, System.Type enable, System.Type disable, GameObject upgradeObject, GameObject ui, GameObject upgradedUi, string tag)
    {
        currentCost = cost;
        enableScript = enable;
        disableScript = disable;
        currentUpgrade = upgradeObject;
        currentUI = ui;
        upgradedUI = upgradedUi;
        currentTag = tag;
    }

    private void ProcessUpgrade(int cost, System.Type enableScript, System.Type disableScript, GameObject upgradeObject, GameObject currentUI, GameObject upgradedUI)
    {
        if (abilityAmountScript.SpendOrbs(cost))
        {
            PlaySound(successSound);

            var enableComponent = GetComponent(enableScript) as MonoBehaviour;
            if (enableComponent != null) enableComponent.enabled = true;

            var disableComponent = GetComponent(disableScript) as MonoBehaviour;
            if (disableComponent != null) disableComponent.enabled = false;

            if (upgradedUI != null) upgradedUI.SetActive(false);
            if (currentUI != null) currentUI.SetActive(true);

            Destroy(upgradeObject);
            abilityAmountScript.SaveOrbs();
        }
        else
        {
            PlaySound(failureSound);
            if (currentTag == "JumpUpgrade")
                StartCoroutine(ShowNoticesTemporarily(jumpNotice));
            else if (currentTag == "DashUpgrade")
                StartCoroutine(ShowNoticesTemporarily(dashNotice));
            else if (currentTag == "TimeSlowUpgrade")
                StartCoroutine(ShowNoticesTemporarily(timeNotice));
        }
    }

    private IEnumerator ShowNoticesTemporarily(GameObject specificNotice)
    {
        // Enable both the general and specific notices
        if (notice != null) notice.SetActive(true);
        if (specificNotice != null) specificNotice.SetActive(true);

        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);

        // Disable both notices
        if (notice != null) notice.SetActive(false);
        if (specificNotice != null) specificNotice.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}