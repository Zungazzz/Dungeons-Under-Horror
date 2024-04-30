using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PickUp, ICollectible
{
    private PlayerStats playerStats;
    public int healthToRestore;
public void Collect()
{
    if (playerStats == null)
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found in the scene.");
            return;
        }
    }
    
    playerStats.RestoreHealth(healthToRestore);
    playerStats.UpdateHealthSlider();
}

}
