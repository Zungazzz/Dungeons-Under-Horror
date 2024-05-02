using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    public CharacterScriptableObject characterScriptableObject;
    public float currentHealth;
      public bool isDead
    {
        get;
        private set;
    }
    public float currentRecovery;
    public float currentMoveSpeed;
    public float currentMight;
    public float currentProjectileSpeed;
    public float currentMagnet;
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
    public GameObject firstWeapon,SecondWeapon;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;


    void Awake()
    {
        inventory = GetComponent<Inventory>();
        currentHealth = characterScriptableObject.MaxHealth;
        currentRecovery = characterScriptableObject.Recovery;
        currentMoveSpeed = characterScriptableObject.MoveSpeed;
        currentMight = characterScriptableObject.Might;
        currentProjectileSpeed = characterScriptableObject.ProjectileSpeed;
        currentWeapon = characterScriptableObject.StartingWeapon;
        currentMagnet = characterScriptableObject.Magnet;
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        SpawnWeapon(firstWeapon);
        SpawnWeapon(SecondWeapon);
        SpawnPassiveItems(firstPassiveItemTest);
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


    public void TakeDamage(float damageAmount, Transform hitTransform)
    {
        if (!isInvincible)
        {
            ScreenShakeManager.Instance.ShakeScreen();
            knockback.GetKnockBack(hitTransform, knockBackThrustAmount);
            StartCoroutine(flash.FlashRoutine());

            cantakeDamage = false;
            currentHealth -= damageAmount;
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            UpdateHealthSlider();
            if (currentHealth <= 0)
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

        
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
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
        }
    }
    void Recovery()
    {
        if (currentHealth < characterScriptableObject.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
            if (currentHealth > characterScriptableObject.MaxHealth)
            {
                currentHealth = characterScriptableObject.MaxHealth;
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
