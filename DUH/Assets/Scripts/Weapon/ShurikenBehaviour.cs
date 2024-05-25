using UnityEngine;
using System.Collections;

public class ShurikenBehaviour : ProjectileWeaponBehaviour
{   
    [SerializeField]
    public int numberOfProjectiles; // Number of projectiles to instantiate
    [SerializeField]
    public float revolvingInterval; // Time interval for revolving around the player
    [SerializeField]
    public float vanishAfterInterval; // Time interval after which the shuriken vanishes
    [SerializeField]
    private float currentRevolveTime; // Timer to track revolving time
    private bool isVanishing; // Flag to track if shuriken is vanishing

    [SerializeField]
    private float cooldownDuration; // Cooldown duration before starting to revolve

    protected override void Start()
    {
        base.Start();

        currentRevolveTime = 0f;
        isVanishing = false;

        // Start cooldown before starting revolving
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);

        // Start revolving around the player
        StartCoroutine(RevolveAroundPlayerCoroutine());
    }

IEnumerator RevolveAroundPlayerCoroutine()
{
    Transform playerTransform = FindObjectOfType<PlayerController>().transform;

    // Get the player's current position and previous position
    Vector3 playerCurrentPos = playerTransform.position;
    Vector3 playerPrevPos = playerCurrentPos;

    // Calculate the player's movement direction
    Vector3 playerMovementDirection = playerCurrentPos - playerPrevPos;
    playerMovementDirection.z = 0; // Ensure it's in 2D

    // Calculate positions for each projectile behind the player's movement
    float angleStep = 360f / numberOfProjectiles;
    float angle = 0f;

    for (int i = 0; i < numberOfProjectiles; i++)
    {
        // Calculate the position behind the player's movement
        Vector3 projectilePos = playerCurrentPos + playerMovementDirection.normalized;

        // Instantiate and set direction for each projectile
        GameObject projectile = Instantiate(weaponData.Prefab, projectilePos, Quaternion.identity);
        projectile.GetComponent<ProjectileWeaponBehaviour>().DirectionChecker(-playerMovementDirection.normalized); // Use -playerMovementDirection.normalized for opposite direction

        angle += angleStep;

        // Yield for the next frame
        yield return new WaitForSeconds(revolvingInterval);
    }

    // Set the flag to start vanishing after revolving
    isVanishing = true;
}

}
