using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityAmount : MonoBehaviour
{
    public int orbAmount = 0; // Current amount of orbs
    public Text orbText1; 
    public Text orbText2; 

    void Start()
    {
        LoadOrbs(); // Load the saved orb amount at the start
        UpdateOrbTexts(); // Update the text fields to reflect the initial orb amount
    }

    public void AddOrbs(int amount)
    {
        orbAmount += amount;
        SaveOrbs(); // Save orbs whenever added
        UpdateOrbTexts(); // Update the UI
    }

    public bool SpendOrbs(int amount)
    {
        if (orbAmount >= amount)
        {
            orbAmount -= amount;
            SaveOrbs(); // Save orbs whenever spent
            UpdateOrbTexts(); // Update the UI
            return true; // Sufficient orbs
        }
        return false; // Insufficient orbs
    }

    public void SaveOrbs()
    {
        PlayerPrefs.SetInt("OrbAmount", orbAmount); // Save orb amount to PlayerPrefs
        PlayerPrefs.Save(); // Ensure changes are written to disk
    }

    public void LoadOrbs()
    {
        orbAmount = PlayerPrefs.GetInt("OrbAmount", 0); // Load orb amount from PlayerPrefs, default to 0 if not set
        UpdateOrbTexts(); // Update the UI after loading
    }

    private void UpdateOrbTexts()
    {
        // Update both UI Text elements with the current orb amount
        if (orbText1 != null)
        {
            orbText1.text = orbAmount.ToString();
        }

        if (orbText2 != null)
        {
            orbText2.text = orbAmount.ToString();
        }
    }
}