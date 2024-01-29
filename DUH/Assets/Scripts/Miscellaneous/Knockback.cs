using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool gettingKnockback {get; private set;}
    [SerializeField] private float knockbackTime = .2f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void GetKnockBack(Transform damagesource, float knockbackThrust)
    {
        gettingKnockback = true;
        Vector2 difference = (transform.position - damagesource.position).normalized * knockbackThrust*rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());

    }
    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
        gettingKnockback = false;
    }
}
