using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemyData;
    Transform player;
    [SerializeField] private float chaseRadius = 5f; // Adjust this value in the Inspector
    [SerializeField] private bool chaseForever = false; // Set this to true if you want the enemy to chase forever once detected

    private bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        enemyData = GetComponent<EnemyStats>(); // Initialize enemyData
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!playerDetected && distanceToPlayer <= chaseRadius)
            {
                playerDetected = true;
            }

            if (playerDetected || distanceToPlayer <= chaseRadius)
            {
                MoveTowardsPlayer();
                AdjustEnemyFacingDirection();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (enemyData != null && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.currentMoveSpeed * Time.deltaTime);
        }
    }

    public void AdjustEnemyFacingDirection()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;

            if (directionToPlayer.x < 0)
            {
                // Face left
                transform.localScale = new Vector3(-1, 1, 1); // This assumes the enemy is facing right by default
            }
            else
            {
                // Face right
                transform.localScale = new Vector3(1, 1, 1); // This assumes the enemy is facing right by default
            }
        }
    }
}
