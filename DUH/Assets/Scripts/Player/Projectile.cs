using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;

    [SerializeField] private float projectileRange = 10f;
    private Vector3 startPosition;
    public WeaponInfo weaponInfo;
    private float damage;

    private void Start()
    {
        startPosition = transform.position;
        damage = weaponInfo.weaponDamage;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float newProjectileRange)
    {
        projectileRange = newProjectileRange;
    }
        public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            Debug.Log("Player hit by projectile");
            playerStats.TakeDamage(damage, transform);
        }
        // Instantiate hit particle effect
        Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
        // Destroy the projectile
        Destroy(gameObject);
    }
    else if (!other.isTrigger)
    {
        EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();

        if (!isEnemyProjectile && (enemyStats || indestructible))
        {
            if (enemyStats || indestructible)
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}



    private void DetectFireDistance()
    {
        if (Vector2.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
