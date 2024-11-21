using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAmount : MonoBehaviour
{
    public int orbAmount = 0; // Current amount of orbs

    public void AddOrbs(int amount)
    {
        orbAmount += amount;
        SaveOrbs(); // Save orbs whenever added
    }

    public bool SpendOrbs(int amount)
    {
        if (orbAmount >= amount)
        {
            orbAmount -= amount;
            SaveOrbs(); // Save orbs whenever spent
            return true; // Sufficient orbs
        }
        return false; // Insufficient orbs
    }

    // Make the SaveOrbs method public so it can be accessed by other classes
    public void SaveOrbs()
    {
        PlayerPrefs.SetInt("OrbAmount", orbAmount); // Save orb amount to PlayerPrefs
        PlayerPrefs.Save(); // Ensure changes are written to disk
    }

    public void LoadOrbs()
    {
        orbAmount = PlayerPrefs.GetInt("OrbAmount", 0); // Load orb amount from PlayerPrefs, default to 0 if not set
    }
}
