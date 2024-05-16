using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    private bool isAttacking = false;
    private bool isOnCooldown = false; // Flag to track whether the weapon is on cooldown
    [SerializeField] private float attackCooldownDuration; // Cooldown duration in seconds

    private PlayerControls playerControls;
    private Animator myAnimator;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
        attackCooldownDuration = weaponInfo.weaponCooldown;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Combat.Attack.performed += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Combat.Attack.performed -= _ => StartAttacking();
        playerControls.Combat.Attack.canceled -= _ => StopAttacking();
    }

    private void StartAttacking()
    {
        if (!isAttacking && !isOnCooldown)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine());
        }
    }

    private void StopAttacking()
    {
        isAttacking = false;
    }

    private IEnumerator AttackRoutine()
    {
        while (isAttacking && !isOnCooldown)
        {
            Attack();
            yield return null;
        }
    }

    public void Attack()
    {
        if (!isOnCooldown)
        {
            myAnimator.SetTrigger(FIRE_HASH);

            // Instantiate a new arrow projectile
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);

            StartCoroutine(AttackCooldownCoroutine()); // Start the cooldown coroutine
        }
    }

    // Coroutine to handle attack cooldown
    private IEnumerator AttackCooldownCoroutine()
    {
        isOnCooldown = true; // Set the cooldown flag
        yield return new WaitForSeconds(attackCooldownDuration);
        isOnCooldown = false; // Reset the cooldown flag after the cooldown duration
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
