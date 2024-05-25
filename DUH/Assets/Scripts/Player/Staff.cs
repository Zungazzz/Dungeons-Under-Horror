using System.Collections;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaserPrefab;
    [SerializeField] private Transform magicLaserSpawnPoint;

    AudioManager audioManager;


    private Animator myAnimator;
    private bool isOnCooldown = false; // Flag to track whether the weapon is on cooldown
    [SerializeField] private float attackCooldownDuration = 0.5f; // Cooldown duration in seconds

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private PlayerControls playerControls;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        attackCooldownDuration = weaponInfo.weaponCooldown;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }
    
    // Coroutine to handle attack cooldown
    private IEnumerator AttackCooldownCoroutine()
    {
        isOnCooldown = true; // Set the cooldown flag
        yield return new WaitForSeconds(attackCooldownDuration);
        isOnCooldown = false; // Reset the cooldown flag after the cooldown duration
    }

    public void Attack()
    {
        // Check if the weapon is on cooldown before attacking
        if (!isOnCooldown)
        {
            myAnimator.SetTrigger(ATTACK_HASH);
            SpawnStaffProjectileAnimEvent();
            audioManager.PlaySFX(audioManager.LaserBeam);
            StartCoroutine(AttackCooldownCoroutine()); // Start the cooldown coroutine
        }
    }

    // This method is called by the attack animation to spawn the magic laser projectile
    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newLaser = Instantiate(magicLaserPrefab, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
