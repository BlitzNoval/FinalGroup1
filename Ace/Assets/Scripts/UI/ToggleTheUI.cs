using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTheUI : MonoBehaviour
{
    public GameObject settings; // The Settings GameObject
    public GameObject credits;  // The Credits GameObject

    // Method to enable the Settings GameObject
    public void EnableSettings()
    {
        if (settings != null)
        {
            settings.SetActive(true);
        }
    }

    // Method to disable the Settings GameObject
    public void DisableSettings()
    {
        if (settings != null)
        {
            settings.SetActive(false);
        }
    }

    // Method to enable the Credits GameObject
    public void EnableCredits()
    {
        if (credits != null)
        {
            credits.SetActive(true);
        }
    }

    // Method to disable the Credits GameObject
    public void DisableCredits()
    {
        if (credits != null)
        {
            credits.SetActive(false);
        }
    }
}
