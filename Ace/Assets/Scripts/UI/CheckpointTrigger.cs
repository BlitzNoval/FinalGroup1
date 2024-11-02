using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointTrigger : MonoBehaviour
{
    public GameObject checkpointUI; 
    public float displayDuration = 2f; 

    private void Start()
    {
        if (checkpointUI != null)
        {
            checkpointUI.SetActive(false); 
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) 
        {
            if (checkpointUI != null)
            {
                StartCoroutine(DisplayCheckpointUI());
            }
        }
    }

    private IEnumerator DisplayCheckpointUI()
    {
        checkpointUI.SetActive(true); 
        yield return new WaitForSeconds(displayDuration); 
        checkpointUI.SetActive(false); 
    }
}
