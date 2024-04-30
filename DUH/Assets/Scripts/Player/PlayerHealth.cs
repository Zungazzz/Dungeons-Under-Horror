using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats playerStats;
    private float maxHealth;
    private float currentHealth;
    // private Knockback knockback;
    // private Flash flash;

    

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        maxHealth = playerStats.characterScriptableObject.MaxHealth;
        // knockback = GetComponent<Knockback>();
        // flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
        if (enemyStats && playerStats.cantakeDamage)
        {
            // Get the damage value directly from the EnemyStats script
            float damage = enemyStats.currentDamage;

            // Apply damage to the player
            playerStats.TakeDamage(damage,transform);

            // // Apply knockback and flash effects
            // knockback.GetKnockBack(other.gameObject.transform, knockbackThrustAmount);
            // StartCoroutine(flash.FlashRoutine());

            // Prevent taking damage temporarily
            playerStats.cantakeDamage = false;

            // // Start coroutine to enable taking damage again after recovery time
            // StartCoroutine(DamageRecoveryRoutine());
        }
    }

    // private IEnumerator DamageRecoveryRoutine()
    // {
    //     yield return new WaitForSeconds(playerStats.currentRecovery);
    //     playerStats.cantakeDamage = true;
    // }
}
