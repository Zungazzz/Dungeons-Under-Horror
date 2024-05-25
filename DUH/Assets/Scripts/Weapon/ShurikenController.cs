using UnityEngine;

public class ShurikenController : WeaponController
{
    [SerializeField]
    public int numberOfProjectiles = 2; // Preset value for number of projectiles
    [SerializeField]
    public float revolvingInterval = 0.5f; // Preset value for revolving interval
    public float vanishAfterInterval = 2f; // Preset value for vanish after interval

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        if (weaponData == null)
        {
            Debug.LogError("weaponData is null!");
            return;
        }

        // Instantiate the Shuriken GameObject at the player's position
        GameObject shuriken = Instantiate(weaponData.Prefab, playerController.transform.position, Quaternion.identity);

        // Configure ShurikenBehavior properties
        ShurikenBehaviour shurikenBehaviour = shuriken.GetComponent<ShurikenBehaviour>();
        shurikenBehaviour.numberOfProjectiles = numberOfProjectiles;
        shurikenBehaviour.revolvingInterval = revolvingInterval;
        Destroy(shuriken, vanishAfterInterval);
    }
}
