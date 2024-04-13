using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    private bool attackButtonDown, isAttacking = false;

    private PlayerControls playerControls;
    private Animator myAnimator;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
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
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void StartAttacking()
    {
        if (!isAttacking)
        {
            attackButtonDown = true;
            Attack();
        }
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

public void Attack()
{
    if (attackButtonDown && !isAttacking)
    {
        isAttacking = false;
        myAnimator.SetTrigger(FIRE_HASH);

        // Instantiate a new arrow projectile
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        StartCoroutine(DestroyArrowAfterDelay(newArrow));
    }
}


    private IEnumerator DestroyArrowAfterDelay(GameObject arrow)
    {
        yield return new WaitForSeconds(2f); // Change 3f to the desired delay in seconds
        Destroy(arrow);
        isAttacking = false;
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
