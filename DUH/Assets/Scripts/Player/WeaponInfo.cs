// WeaponInfo.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public int weaponDamage; // New variable to store base damage
    public float weaponRange;

    // Method to calculate final weapon damage including player's "might"
    public int GetFinalWeaponDamage(float playerMight)
    {
        return Mathf.RoundToInt(weaponDamage * playerMight);
    }
}
