using UnityEngine;

public class ActivateEverything : MonoBehaviour
{
    // Reference to the block that will be activated
    public GameObject targetBlock;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the Player tag
        if (other.CompareTag("Player") && targetBlock != null)
        {
            // Activate the entire target block
            targetBlock.SetActive(true);

            // Optionally play the animation if the block has an Animator
            Animator targetAnimator = targetBlock.GetComponent<Animator>();
            if (targetAnimator != null)
            {
                targetAnimator.enabled = true;
                targetAnimator.Play(0); // Plays the default animation
            }
        }
    }
}
