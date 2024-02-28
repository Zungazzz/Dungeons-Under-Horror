using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{   
    List<GameObject> markedEnemies = new List<GameObject>();
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
protected override void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Enemy"))
    {
        EnemyStats enemy = collision.gameObject.GetComponent<EnemyStats>();
        if (enemy != null && !markedEnemies.Contains(collision.gameObject))
        {
            enemy.TakeDamage(currentDamage);
            markedEnemies.Add(collision.gameObject);
        }
    }
    else if (collision.CompareTag("Prop"))
    {
        BreakableProps breakable = collision.gameObject.GetComponent<BreakableProps>();
        if (breakable != null && !markedEnemies.Contains(collision.gameObject))
        {
            breakable.TakeDamage(currentDamage);
            markedEnemies.Add(collision.gameObject);
        }
    }
}



}
