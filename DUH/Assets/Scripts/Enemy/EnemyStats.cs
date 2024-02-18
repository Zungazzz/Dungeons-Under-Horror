using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyStats;

    float currentMoveSpeed;
    float currentHealth;
    float currentDamage;

    private void Awake(){
        currentMoveSpeed = enemyStats.MoveSpeed;
        currentHealth = enemyStats.MaxHealth;
        currentDamage = enemyStats.Damage;
    }
    public void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Kill();
        }
    }
    public void Kill(){
        Destroy(gameObject);
    }
}
