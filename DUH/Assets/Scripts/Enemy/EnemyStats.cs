using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{   
    [SerializeField] private float knockBackThrust = 15f;
    private Knockback knockback;
    private Flash flash; 
    [SerializeField] private GameObject deathVFXPrefab;
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
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }
    public void TakeDamage(float damage){

        currentHealth -= damage;
        if (knockback != null)
        {
            knockback.GetKnockBack(PlayerController.Instance.transform, knockBackThrust);
        }

        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        if(currentHealth <= 0){

            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
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
