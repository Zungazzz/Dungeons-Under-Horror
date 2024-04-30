using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionOfPlayer { get; private set; }

    [SerializeField]private float _playerAwarenessDistance;
    private Transform _playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Player Transform: " + _playerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerPosition = _playerTransform.position - transform.position; 
        DirectionOfPlayer = enemyToPlayerPosition.normalized;

        if (enemyToPlayerPosition.magnitude <= _playerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }
        else
        {
            AwareOfPlayer = false;
        }
    }
}
