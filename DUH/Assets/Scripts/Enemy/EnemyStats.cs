using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyStats;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

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
    private void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }
}
