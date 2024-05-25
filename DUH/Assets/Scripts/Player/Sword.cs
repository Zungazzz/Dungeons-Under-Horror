using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private bool attackButtonDown, isAttacking = false;
    private GameObject slashAnim;
    private bool isSwingUp = true; // Track the current attack type

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD;
    [SerializeField] private WeaponInfo weaponInfo;
    AudioManager audioManager;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        swordAttackCD = weaponInfo.weaponCooldown;
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        playerControls.Enable();
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    public WeaponInfo GetWeaponInfo() 
    {
        return weaponInfo;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Combat.Attack.started -= _ => StartAttacking();
        playerControls.Combat.Attack.canceled -= _ => StopAttacking();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Update()
    {
        MouseFollowWithOffset();
        if (attackButtonDown && !isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        Debug.Log("Attack initiated");
        isAttacking = true;
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = transform.parent;

        StartCoroutine(AttackCDCoroutine());
    }

    private IEnumerator AttackCDCoroutine()
    {   
        audioManager.PlaySFX(audioManager.Sword);
        Debug.Log("Starting cooldown coroutine");
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
        Debug.Log("Cooldown complete, ready to attack again");
    }

    public void DoneAttackAnimationEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        Debug.Log("SwingUpFlipAnimEvent called");
        if (slashAnim != null)
        {
            slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        Debug.Log("SwingDownFlipAnimEvent called");
        if (slashAnim != null)
        {
            slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
