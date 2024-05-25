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
    public float currentMoveSpeed;
    public float currentHealth;
    public float currentDamage;
    private EnemyHealthUI enemyHealthUI;
    AudioManager audioManager;

    private void Awake()
    {
        currentMoveSpeed = enemyStats.MoveSpeed;
        currentHealth = enemyStats.MaxHealth;
        currentDamage = enemyStats.Damage;
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        enemyHealthUI = GetComponent<EnemyHealthUI>();
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();

    }
    public void TakeDamage(float damage)
    {

        currentHealth -= damage;
        if (damage > 0)
        {
            GameManager.GenerateFloatingText($"{Mathf.FloorToInt(damage)}", transform);
        }
        if (knockback != null)
        {
            knockback.GetKnockBack(PlayerController.Instance.transform, knockBackThrust);
        }

        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        if (enemyHealthUI != null)
        {
            enemyHealthUI.UpdateHealthSlider(currentHealth, enemyStats.MaxHealth);
        }
        if (currentHealth <= 0)
        {

            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Kill();
        }
    }
    public void Kill()
    {
        audioManager.PlaySFX(audioManager.Death);
        Destroy(gameObject);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform);
        }
    }
}
