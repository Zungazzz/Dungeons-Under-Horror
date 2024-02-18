using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBehaviour : ProjectileWeaponBehaviour
{

    public float rotationSpeed = 360f; // Adjust the angular speed of rotation as needed

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        // Calculate rotation based on time
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Move the projectile in the specified direction
        transform.position += direction * weaponData.Speed * Time.deltaTime;
    }
}
