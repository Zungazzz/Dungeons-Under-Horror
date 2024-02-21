using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats playerStats;
    CircleCollider2D playerCollector;
    public float pullForce;
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        playerCollector.radius = playerStats.currentMagnet;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<ICollectible>(out ICollectible collectible))
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = transform.position - col.transform.position;
            rb.AddForce(forceDirection * pullForce);
            collectible.Collect();
        }
    }
}
