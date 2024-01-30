using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour

{
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private float knockBackThrust = 15f;
    private int currentHealth;
    private Knockback knockback;
    private Flash flash; 

    private void Start()
    {
        currentHealth = startingHealth;
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>(); 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (knockback != null)
        {
            knockback.GetKnockBack(PlayerController.Instance.transform, knockBackThrust);
        }

        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }

        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine(){
        yield return new WaitForSeconds(flash.GetRestoreMatTIme());
        DetectDeath();
    }
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
