using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;

    private PlayerStats playerStats; // Variable to hold PlayerStats component
    public WeaponInfo weaponInfo; // Assuming you have a WeaponInfo component attached to the same GameObject

    private void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();
    }

    void Start()
    {
        StartCoroutine(spriteFade.SlowFadeRoutine());
        Invoke("DisableCollider", 0.2f); 
    }

private void OnTriggerEnter2D(Collider2D other)
{
    PlayerStats playerStats = other.GetComponent<PlayerStats>();
    if (playerStats != null)
    {
        // Apply damage to the player
        playerStats.TakeDamage(weaponInfo.weaponDamage, transform);
    }
}


    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
