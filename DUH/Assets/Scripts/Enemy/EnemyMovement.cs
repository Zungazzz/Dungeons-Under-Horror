using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemyData;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        enemyData = GetComponent<EnemyStats>(); // Initialize enemyData
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        AdjustEnemyFacingDirection();
    }

    void MoveTowardsPlayer()
    {
        if (enemyData != null && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.currentMoveSpeed * Time.deltaTime);
        }
    }

    void AdjustEnemyFacingDirection()
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
