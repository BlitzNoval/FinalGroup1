using UnityEngine;
using System.Collections;
using TMPro;

public class DashAbility : MonoBehaviour
{
    public string abilityName = "Dash";
    public float dashLength = 0.3f;       // Shortened dash duration for snappiness
    public float dashSpeed = 15f;         // Increased dash speed
    public int maxUses = 3;
    public float cooldownTime = 1f;       // Cooldown time between dashes
    public ParticleSystem dashEffect;
    public AudioClip dashSound;           // Sound effect for dashing
    public TextMeshProUGUI abilityText;

    private int remainingUses;
    private bool abilityActive = false;
    private bool isOnCooldown = false;
    private Vector3 dashDirection;
    private Rigidbody playerRb;
    private AudioSource audioSource;

    private void Start()
    {
        remainingUses = maxUses;
        playerRb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
        UpdateAbilityUI();
    }

    private void Update()
    {
        if (abilityActive && remainingUses > 0 && Input.GetKeyDown(KeyCode.E) && !isOnCooldown)
        {
            PerformDash();
        }
        UpdateAbilityUI();
    }

    public void ActivateAbility()
    {
        abilityActive = true;
        remainingUses = maxUses;
        UpdateAbilityUI();
    }

    private void PerformDash()
    {
        remainingUses--;

        if (playerRb.velocity.magnitude > 0.1f)
        {
            dashDirection = playerRb.velocity.normalized * dashSpeed;
        }
        else
        {
            dashDirection = transform.forward * dashSpeed;
        }

        StartCoroutine(DashCoroutine());

        if (dashEffect)
        {
            dashEffect.Play();
        }

        if (dashSound)
        {
            audioSource.PlayOneShot(dashSound);
        }

        if (remainingUses <= 0)
        {
            DeactivateAbility();
        }

        StartCoroutine(DashCooldown());

        UpdateAbilityUI();
    }

    private IEnumerator DashCoroutine()
    {
        float elapsedTime = 0f;
        playerRb.useGravity = false;

        while (elapsedTime < dashLength)
        {
            playerRb.velocity = dashDirection;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerRb.velocity = Vector3.zero;
        playerRb.useGravity = true;
    }

    private IEnumerator DashCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    private void DeactivateAbility()
    {
        abilityActive = false;
        remainingUses = 0;
        Debug.Log("Dash ability has been used up.");
        UpdateAbilityUI();
    }

    private void UpdateAbilityUI()
    {
        if (abilityActive)
        {
            abilityText.text = $"Ability: {abilityName} ({remainingUses} uses remaining)";
            abilityText.color = isOnCooldown ? Color.gray : Color.white; // Change color if on cooldown
        }
        else
        {
            abilityText.text = "Ability: Empty";
        }
    }
}