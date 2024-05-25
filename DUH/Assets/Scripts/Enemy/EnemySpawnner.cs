using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float minimumSpawnTime;
    [SerializeField]
    private float maximumSpawnTime;
    [SerializeField]
    private int numberofEnemies;

    private float timeUntilSpawn;
    private int enemiesSpawned;

    void Awake()
    {
        SetTimeUntilSpawn();
        enemiesSpawned = 0;
    }

    private void Update()
    {
        if (enemiesSpawned < numberofEnemies)
        {
            timeUntilSpawn -= Time.deltaTime;
            if (timeUntilSpawn <= 0)
            {
                SpawnEnemy();
                SetTimeUntilSpawn();
            }
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemiesSpawned++;
    }
}
