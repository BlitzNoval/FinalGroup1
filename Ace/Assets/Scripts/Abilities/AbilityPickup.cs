using UnityEngine;
using UnityEngine.UI; // For UI elements like text
using TMPro;

public class AbilityPickup : MonoBehaviour
{
    public DashAbility dashAbility; // The player's dash ability component
    public TextMeshProUGUI abilityText; // UI Text component to display the ability name

    private void Start()
    {
        // Initially display that no ability is active
        abilityText.text = "Ability: Empty";
    }

    // Trigger when the player walks into the pickup object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the object
        if (other.CompareTag("Player"))
        {
            // Activate the dash ability for the player
            dashAbility.ActivateAbility();

            // Update UI to show the ability name
            abilityText.text = "Ability: " + dashAbility.abilityName;

            // Optionally, destroy the pickup object after it's collected
            Destroy(gameObject);
        }
    }
}
