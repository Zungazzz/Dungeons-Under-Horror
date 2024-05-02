// DamageSource.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the EnemyStats component from the collided object
        EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();

        // Check if enemyStats is not null before attempting to call TakeDamage
        if (enemyStats != null)
        {
            // Call the TakeDamage method on the retrieved EnemyStats instance
            enemyStats.TakeDamage(weaponInfo.GetFinalWeaponDamage(FindObjectOfType<PlayerStats>().currentMight)); // Pass player's "might"
        }

        BreakableProps breakableProps = other.gameObject.GetComponent<BreakableProps>();

        // If breakableProps is not null, destroy the breakable object
        if (breakableProps != null)
        {
            // Destroy the breakable object
            Destroy(other.gameObject);
        }
    }
}
