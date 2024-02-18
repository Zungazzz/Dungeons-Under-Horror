using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyScriptableObjects", menuName = "ScriptableObject/EnemyScriptableObject", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    //Stats for the enemy
    [SerializeField]
    string enemyName;
    public string EnemyName
    {
        get => enemyName; private set => enemyName = value;
    }
    [SerializeField]
    float moveSpeed;
    public  float MoveSpeed
    {
        get => moveSpeed; private set => moveSpeed = value;
    }
    [SerializeField]
    float maxHealth;
    public float MaxHealth
    {
        get => maxHealth; private set => maxHealth = value;
    }
    [SerializeField]
    float damage;
    public float Damage
    {
        get => damage; private set => damage = value;
    }
}
