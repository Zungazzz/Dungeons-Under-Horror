using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterScriptableObject;
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    GameObject currentWeapon;
    public List<LevelRange> levelRanges;

    [Header("Experience/Level")]
    public int level = 1;
    public int experience = 0;
    public int experienceCap;

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    void Awake()
    {
        currentHealth = characterScriptableObject.MaxHealth;
        currentRecovery = characterScriptableObject.Recovery;
        currentMoveSpeed = characterScriptableObject.MoveSpeed;
        currentMight = characterScriptableObject.Might;
        currentProjectileSpeed = characterScriptableObject.ProjectileSpeed;
        currentWeapon = characterScriptableObject.StartingWeapon;
    }

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    void Start()
    {
        // Initialize the experience cap
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }


    public void Kill()
    {
        Debug.Log("Player has died");
    }
    public void RestoreHealth(float amount)
    {
    if (currentHealth < characterScriptableObject.MaxHealth)
    {
        currentHealth += amount;
    }
    if (currentHealth > characterScriptableObject.MaxHealth)
    {
        currentHealth = characterScriptableObject.MaxHealth;
    }}
}
