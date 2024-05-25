using UnityEngine;

public class MaxHealthPassiveItems : PassiveItems
{
    protected override void ApplyModifiers()
    {
        // Ensure playerStats and passiveItemsScriptableObject are not null
        if (playerStats != null && passiveItemsScriptableObject != null)
        {
            // Increase the player's MaxHealth by a percentage based on the passive item multiplier
            float additionalHealth = playerStats.characterScriptableObject.MaxHealth * (passiveItemsScriptableObject.Multiply / 100f);
            playerStats.characterScriptableObject.MaxHealth += additionalHealth;
            playerStats.CurrentHealth += additionalHealth; // Optionally increase current health as well

        }
    }
}
