using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public Image expBar;
    public Text levelText;
    public CharacterScriptableObject characterScriptableObject;
    float currentHealth;
    float currentMagnet;
    float currentMoveSpeed;
    float currentRecovery;
    float currentProjectileSpeed;
    float currentMight;
    public bool isDead
    {
        get;
        private set;
    }
    #region CurrentStats
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {

            if (currentHealth != value)
            {

                currentHealth = value;
                UpdateHealthSlider();
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthText.text = "Health: " + currentHealth.ToString();
                }

            }
        }
    }


    public float CurrentRecovery
    {
        get
        {
            return currentRecovery;
        }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryText.text = "Recovery: " + currentRecovery.ToString();
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            return currentMoveSpeed;
        }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedText.text = "Move Speed: " + currentMoveSpeed.ToString();
                }
            }
        }
    }

    public float CurrentMight
    {
        get
        {
            return currentMight;
        }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightText.text = "Might: " + currentMight.ToString();
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get
        {
            return currentProjectileSpeed;
        }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedText.text = "Projectile Speed: " + currentProjectileSpeed.ToString();
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get
        {
            return currentMagnet;
        }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetText.text = "Magnet: " + currentMagnet.ToString();
                }
            }
        }
    }
    #endregion
    GameObject currentWeapon;
    public List<LevelRange> levelRanges;
    private Slider healthSlider;

    [Header("Experience/Level")]
    public int level = 1;
    public int experience = 0;
    public int experienceCap;
    public bool cantakeDamage = true;

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;
    private Knockback knockback;
    private Flash flash;
    [SerializeField] private float knockBackThrustAmount = 10f;
    Inventory inventory;
    public int weaponIndex;
    public int passiveItemIndex;
    public GameObject firstWeapon, SecondWeapon;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;


    void Awake()
    {
        inventory = GetComponent<Inventory>();
        CurrentHealth = characterScriptableObject.MaxHealth;
        CurrentRecovery = characterScriptableObject.Recovery; // Set CurrentRecovery
        CurrentMoveSpeed = characterScriptableObject.MoveSpeed;
        CurrentMight = characterScriptableObject.Might;
        CurrentProjectileSpeed = characterScriptableObject.ProjectileSpeed;
        currentWeapon = characterScriptableObject.StartingWeapon;
        CurrentMagnet = characterScriptableObject.Magnet; // Set CurrentMagnet
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        SpawnWeapon(firstWeapon);
        // SpawnWeapon(SecondWeapon);
        // SpawnPassiveItems(firstPassiveItemTest);
        SpawnPassiveItems(secondPassiveItemTest);
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
        isDead = false;
        // Initialize the experience cap
        experienceCap = levelRanges[0].experienceCapIncrease;
        UpdateHealthSlider();
        GameManager.instance.currentHealthText.text = "Health: " + currentHealth.ToString();
        GameManager.instance.currentRecoveryText.text = "Recovery: " + currentRecovery.ToString();
        GameManager.instance.currentMoveSpeedText.text = "Move Speed: " + currentMoveSpeed.ToString();
        GameManager.instance.currentMightText.text = "Might: " + currentMight.ToString();
        GameManager.instance.currentProjectileSpeedText.text = "Projectile Speed: " + currentProjectileSpeed.ToString();
        GameManager.instance.currentMagnetText.text = "Magnet: " + currentMagnet.ToString();

        GameManager.instance.AssignChosenCharacterUI(characterScriptableObject);
        UpdateExpBar();
        UpdateLevelText();
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
            Recovery();
        }
    }


    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();
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
            UpdateLevelText();
            GameManager.instance.StartLevelUp();
        }

    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / (float)experienceCap;
    }
    void UpdateLevelText()
    {
        levelText.text = "Level: " + level.ToString();
    }

    public void TakeDamage(float damageAmount, Transform hitTransform)
    {
        if (!isInvincible)
        {
            if (damageAmount > 0)
            {
                GameManager.GenerateFloatingText($"{Mathf.FloorToInt(damageAmount)}", transform);
            }
            ScreenShakeManager.Instance.ShakeScreen();
            knockback.GetKnockBack(hitTransform, knockBackThrustAmount);
            StartCoroutine(flash.FlashRoutine());

            cantakeDamage = false;
            CurrentHealth -= damageAmount;
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            UpdateHealthSlider();
            if (CurrentHealth <= 0)
            {
                isDead = true;
                Kill();
            }
        }
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(2f);
        cantakeDamage = true;
    }

    public void Kill()
    {
        Debug.Log("Player has died");
        StartCoroutine(DeathLoadSceneRoutine());
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
        }


    }

    private IEnumerator DeathLoadSceneRoutine()
    {

        yield return new WaitForSeconds(1f);
        // SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterScriptableObject.MaxHealth)
        {
            CurrentHealth += amount;
        }
        if (CurrentHealth > characterScriptableObject.MaxHealth)
        {
            CurrentHealth = characterScriptableObject.MaxHealth;
        }
    }
    void Recovery()
    {
        if (CurrentHealth < characterScriptableObject.MaxHealth)
        {
            CurrentHealth += currentRecovery * Time.deltaTime;
            if (CurrentHealth > characterScriptableObject.MaxHealth)
            {
                CurrentHealth = characterScriptableObject.MaxHealth;
            }
        }
    }
    public void UpdateHealthSlider()
    {
        // Find the HealthSlider GameObject in the scene
        GameObject healthSliderObject = GameObject.Find("HealthSlider");

        // Check if the HealthSlider GameObject is found
        if (healthSliderObject != null)
        {
            // Get the Slider component from the HealthSlider GameObject
            Slider sliderComponent = healthSliderObject.GetComponent<Slider>();

            // Check if the Slider component is found
            if (sliderComponent != null)
            {
                // Assign the Slider component to healthSlider
                healthSlider = sliderComponent;

                // Set the value and maxValue of the healthSlider
                healthSlider.value = currentHealth;
                healthSlider.maxValue = characterScriptableObject.MaxHealth;
            }
            else
            {
                Debug.LogWarning("Slider component not found on HealthSlider GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("HealthSlider GameObject not found in the scene.");
        }
    }
    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex < inventory.weaponSlots.Count)
        {
            GameObject newWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            newWeapon.transform.SetParent(transform);
            inventory.AddWeapon(weaponIndex, newWeapon.GetComponent<WeaponController>());

            weaponIndex++;
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public void SpawnPassiveItems(GameObject passiveItem)
    {
        if (passiveItemIndex < inventory.passiveItemsSlots.Count)
        {
            GameObject newPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
            newPassiveItem.transform.SetParent(transform);
            inventory.AddPassiveItem(passiveItemIndex, newPassiveItem.GetComponent<PassiveItems>());

            passiveItemIndex++;
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

}
