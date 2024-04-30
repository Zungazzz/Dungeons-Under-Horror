using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject mortarProjectilePrefab;
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
        Debug.Log("Mortar Attack");
        if (transform.position.x - PlayerController.Instance.transform.position.x<0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    public void SpawnProjectileAnimEvent()
    {
        Instantiate(mortarProjectilePrefab, transform.position, Quaternion.identity);
    }
}
