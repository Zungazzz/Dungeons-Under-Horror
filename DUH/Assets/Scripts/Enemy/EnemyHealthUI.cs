using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    private EnemyStats enemyStats; // Reference to the EnemyStats script

    private Slider healthSlider;

    void Awake()
    {
        // Get the EnemyStats component attached to this GameObject
        enemyStats = GetComponent<EnemyStats>();

        // Initialize current health and max health based on enemyStats
        currentHealth = enemyStats.currentHealth;
        maxHealth = enemyStats.enemyStats.MaxHealth;
        UpdateHealthSlider(currentHealth, maxHealth);
    }

    void Start()
    {

        // Find the HealthSlider GameObject in the scene
        GameObject healthSliderObject = GameObject.Find("Enemy_HealthSlider");

        // Check if the HealthSlider GameObject is found
        if (healthSliderObject != null)
        {
            // Get the Slider component from the HealthSlider GameObject
            healthSlider = healthSliderObject.GetComponent<Slider>();

            // Check if the Slider component is found
            if (healthSlider != null)
            {
                // Update the health slider continuously
                UpdateHealthSlider(enemyStats.currentHealth, enemyStats.enemyStats.MaxHealth);
            }
            else
            {
                Debug.LogWarning("Slider component not found on Enemy_HealthSlider GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Enemy_HealthSlider GameObject not found in the scene.");
        }
    }

    // Method to update the health slider
    public void UpdateHealthSlider(float currentHealth, float maxHealth)
    {
        // Check if the healthSlider reference is valid
        if (healthSlider != null)
        {
            // Set the value and maxValue of the healthSlider
            healthSlider.value = currentHealth;
            healthSlider.maxValue = maxHealth;
        }
    }
}
