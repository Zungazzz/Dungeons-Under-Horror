using System.Collections;
using UnityEngine;

public class MinionBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Animator animator; // Reference to Animator
    [SerializeField] private MonoBehaviour enemyBehaviour;

    private GameObject player;
    private float lastAttackTime;
    private EnemyStats enemyStats;
    private bool isSummoning = true; // To check if the summon animation is playing

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        lastAttackTime = Time.time;
        enemyStats = GetComponent<EnemyStats>();
        StartCoroutine(Summon());
    }

    private void Update()
    {
        if (isSummoning) return; // If summoning animation is playing, don't move

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            // Not chasing the player, add any desired behavior here
        }
        else
        {
            Idle();
        }
    }

    private IEnumerator Summon()
    {
        animator.Play("Summon");
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.Play("SummonIdle");

        isSummoning = false;
        if (enemyBehaviour != null)
        {
            enemyBehaviour.enabled = true;
        }
    }

    private void Idle()
    {
        Vector2 idleMovement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(idleMovement);
    }

    private void Attack()
    {
        Debug.Log("Minion attacks player for " + enemyStats.currentDamage + " damage.");

        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(enemyStats.currentDamage, transform);
        }
    }

    public IEnumerator Die()
    {
        animator.Play("SummonDeath");
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        Destroy(gameObject);
    }
}
