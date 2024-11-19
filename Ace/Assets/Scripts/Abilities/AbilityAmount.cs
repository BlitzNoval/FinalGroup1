using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAmount : MonoBehaviour
{
    public int orbAmount = 0; // Current amount of orbs

    public void AddOrbs(int amount)
    {
        orbAmount += amount;
    }

    public bool SpendOrbs(int amount)
    {
        if (orbAmount >= amount)
        {
            orbAmount -= amount;
            return true; // Sufficient orbs
        }
        return false; // Insufficient orbs
    }
}
