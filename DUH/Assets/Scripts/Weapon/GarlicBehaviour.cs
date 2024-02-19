using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{   
    List<GameObject> markedEnemies;
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
        enemy.TakeDamage(currentDamage);
        markedEnemies.Add(collision.gameObject);
    }
    else if (collision.CompareTag("Prop"))
    {
        if (collision.gameObject.TryGetComponent(out BreakableProps breakable))
        {
            breakable.TakeDamage(currentDamage);
            markedEnemies.Add(collision.gameObject);
        }
    }
}


}
