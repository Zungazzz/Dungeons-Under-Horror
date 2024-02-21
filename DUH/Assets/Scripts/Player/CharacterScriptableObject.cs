using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObject/Character", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject startingWeapon;
    public GameObject StartingWeapon { get => startingWeapon; set => startingWeapon = value; }

    [SerializeField] private float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField] private float recovery;
    public float Recovery { get => recovery; set => recovery = value; }

    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField] private float might;
    public float Might { get => might; set => might = value; }

    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    // Define Magnet property with a private setter
    [SerializeField] private float magnet;
    public float Magnet { get => magnet; private set => magnet = value; }
}
