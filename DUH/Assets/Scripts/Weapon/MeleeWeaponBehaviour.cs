using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    public float destroyAfterSeconds;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPirece;

    void Awake()
    {
        currentCooldownDuration = weaponData.CoolDownDuration;
        currentDamage = weaponData.Damage;
        currentPirece = weaponData.Pierce;
        currentSpeed = weaponData.Speed;
    }

    public float GetCurrentDamage()
    {
        // Modify the current damage value
        currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
        // Return the modified damage value
        return currentDamage;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.gameObject.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(GetCurrentDamage());
            }
        }
        else if (collision.CompareTag("Breakable"))
        {
            BreakableProps breakable = collision.gameObject.GetComponent<BreakableProps>();
            if (breakable != null)
            {
                breakable.TakeDamage(GetCurrentDamage());
            }
        }
    }
}
