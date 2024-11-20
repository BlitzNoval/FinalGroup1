using System.Collections;
using UnityEngine;
using TMPro; 

public class CheckpointTrigger : MonoBehaviour
{
    public TextMeshProUGUI checkpointText; 
    public float displayDuration = 2f; 

    private void Start()
    {
        if (checkpointText != null)
        {
            checkpointText.gameObject.SetActive(false); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkpointText != null)
            {
                StartCoroutine(DisplayCheckpointText());
            }
        }
    }

    private IEnumerator DisplayCheckpointText()
    {
        checkpointText.gameObject.SetActive(true); 
        yield return new WaitForSeconds(displayDuration);
        checkpointText.gameObject.SetActive(false); 
    }
}
