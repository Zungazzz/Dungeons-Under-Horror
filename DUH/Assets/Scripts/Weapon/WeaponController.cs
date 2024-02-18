using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponController : MonoBehaviour

{
protected PlayerController playerController;
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    public float currentCoolDown;

    protected virtual void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        currentCoolDown = weaponData.CoolDownDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCoolDown -= Time.deltaTime;
        if(currentCoolDown<=0f){
            Attack();
        }
    }
    protected virtual void Attack(){
        currentCoolDown = weaponData.CoolDownDuration;
    }
}
