using UnityEngine;

public class EnemyTargetingProjectileBehaviour : ProjectileWeaponBehaviour
{
    private Vector2 launchDirection;

    public void Launch(Vector2 direction, float speed)
    {
        launchDirection = direction;
        currentSpeed = speed;
        Destroy(gameObject, destroyAfterSeconds); // Call base Destroy after seconds
    }

    protected override void Update()
    {
        // Call the base class Update method for default behavior
        base.Update();

        // Move the projectile in the launch direction
        transform.position += (Vector3)launchDirection * currentSpeed * Time.deltaTime;
    }
}
