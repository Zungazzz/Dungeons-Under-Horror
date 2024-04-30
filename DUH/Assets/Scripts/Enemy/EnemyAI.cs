using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }
    private Vector2 roamPosition;
    private float timeRoaming;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    public void Update()
    {
        MovementStateControl();

    }

    public void MovementStateControl()
    {
        switch (state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            default:
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathfinding.MoveTo(roamPosition);
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange)
        {
            state = State.Attacking;
        }
        if (timeRoaming >= roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        // Check if the player is out of attack range, switch to roaming state
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
            return;
        }

        // Check if attack cooldown is finished and the enemy can attack
        if (canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCoolDownRoutine());
        }
    }



    private IEnumerator AttackCoolDownRoutine()
    {
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
