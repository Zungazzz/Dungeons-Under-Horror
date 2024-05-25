using UnityEngine;
using System.Collections;

public class EnemyTargetingWeaponController : WeaponController
{
    public float targetingRange; // Range within which to find enemies

    protected override void Attack()
    {
        base.Attack();

        // Find the nearest enemy within range
        GameObject nearestEnemy = FindNearestEnemy(transform.position, targetingRange);

        if (nearestEnemy != null)
        {
            // Calculate direction to the enemy
            Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;

            // Spawn and launch projectile
            GameObject spawnedProjectile = Instantiate(weaponData.Prefab);
            spawnedProjectile.transform.position = transform.position;
            spawnedProjectile.GetComponent<EnemyTargetingProjectileBehaviour>().Launch(direction, weaponData.Speed);

            // Destroy projectile on next attack (assuming this script is attached to the weapon)
            StartCoroutine(DestroyProjectileOnNextAttack(spawnedProjectile));
        }
    }

    private GameObject FindNearestEnemy(Vector2 position, float range)
    {
        // Use Physics2D.OverlapCircleAll for 2D enemy detection
        Collider2D[] potentialEnemies = Physics2D.OverlapCircleAll(position, range);

        GameObject nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Loop through potential enemies and find the closest one
        foreach (Collider2D collider in potentialEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = collider.gameObject;
                }
            }
        }

        return nearestEnemy;
    }

    private IEnumerator DestroyProjectileOnNextAttack(GameObject projectile)
    {
        yield return new WaitForSeconds(currentCoolDown);
        Destroy(projectile);
    }
}
