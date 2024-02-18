using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnStone = Instantiate(weaponData.Prefab);
        spawnStone.transform.position = transform.position;

        // Get the position of the mouse cursor in the game world
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Ensure the z-coordinate is appropriate for 2D

        // Calculate the direction from the player to the mouse cursor position
        Vector3 throwDirection = (mousePos - transform.position).normalized;

        // Pass the direction to StoneBehaviour
        spawnStone.GetComponent<StoneBehaviour>().DirectionChecker(throwDirection);
    }
}
