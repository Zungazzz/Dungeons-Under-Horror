using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private bool attackButtonDown, isAttacking=false;
    private GameObject slashAnim;
        [SerializeField] private GameObject slashAnimPrefab;
        [SerializeField] private Transform slashAnimSpawnPoint;

        [SerializeField] private Transform weaponCollider;
        [SerializeField] private float swordAttackCD;
        [SerializeField] private WeaponInfo weaponInfo;
    private void Awake()
    {


        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        swordAttackCD = weaponInfo.weaponCooldown;

        // Subscribe to the attack action when the controls are enabled
        playerControls.Enable();
        playerControls.Combat.Attack.started += _ => Attack();
    }
    
    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }


    void Start()
    {
     playerControls.Combat.Attack.started += _ => StartAttacking();
     playerControls.Combat.Attack.canceled += _ => StopAttacking();   
    }

    private void OnDisable()
    {
        // Unsubscribe from the attack action when the controls are disabled
        playerControls.Disable();
        playerControls.Combat.Attack.started -= _ => Attack();
    }

    private void StartAttacking(){
        attackButtonDown = true;
    }
    private void StopAttacking(){
        attackButtonDown = false;
    }

    public void Attack()
    {
        if(attackButtonDown && !isAttacking){
        isAttacking = true;
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = transform.parent;
        StartCoroutine(AttackCDCoroutine());
        }

   }

   private IEnumerator AttackCDCoroutine(){
    yield return new WaitForSeconds(swordAttackCD);
    isAttacking = false;
   }

   public void DoneAttackAnimationEvent(){
    weaponCollider.gameObject.SetActive(false);

   }

    private void Update()
    {

        MouseFollowWithOffset();
        Attack();

    }

    public void SwingUpFlipAnimEvent(){
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if(playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void SwingDownFlipAnimEvent(){
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if(playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);
        
        float angle = Mathf.Atan2(mousePos.y , mousePos.x ) * Mathf.Rad2Deg;  

        
        if(mousePos.x < playerScreenPoint.x)
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
