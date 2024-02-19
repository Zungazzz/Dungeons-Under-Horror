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

    void Awake(){
        currentCooldownDuration=weaponData.CoolDownDuration;
        currentDamage=weaponData.Damage;
        currentPirece=weaponData.Pierce;
        currentSpeed=weaponData.Speed;
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
            enemy.TakeDamage(currentDamage);
        }
        else if (collision.CompareTag("Breakable"))
        {
           if(collision.gameObject.TryGetComponent(out BreakableProps breakable )){
            breakable.TakeDamage(currentDamage);
        }
        }
    }

}