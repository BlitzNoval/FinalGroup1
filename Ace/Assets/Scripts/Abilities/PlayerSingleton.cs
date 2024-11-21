using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    private static PlayerSingleton _instance;

    public static PlayerSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerSingleton>();
                if (_instance == null)
                {
                    Debug.LogError("PlayerSingleton not found!");
                }
            }
            return _instance;
        }
    }

    // Ensure that only one instance exists
    public void SetInstance(GameObject player)
    {
        if (_instance == null)
        {
            _instance = player.GetComponent<PlayerSingleton>();
            DontDestroyOnLoad(player); // Make sure the player persists across scenes
        }
        else
        {
            Destroy(player); // Destroy any extra player instances
        }
    }
}
