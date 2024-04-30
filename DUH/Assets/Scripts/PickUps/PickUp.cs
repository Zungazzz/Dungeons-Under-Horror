using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player does not have a Projectile component
            Projectile projectileComponent = other.GetComponent<Projectile>();
            if (projectileComponent == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
