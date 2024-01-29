using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    private Knockback knockback;   
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movedir;

    private void Awake()
    {   
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockback)
        {
            return;
        }

        rb.MovePosition(rb.position + movedir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPos)
    {
        movedir = (targetPos - (Vector2)transform.position).normalized;
    }
}
