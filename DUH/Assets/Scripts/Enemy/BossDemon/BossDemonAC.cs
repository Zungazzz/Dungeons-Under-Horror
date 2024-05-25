using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDemonAC : MonoBehaviour
{
    public EnemyAI enemyAI;
    public EnemyPathfinding enemyPathFinding;
    public EnemyMovement enemyMovement;
    public Animator animator;
    public float detectionRadius = 10f; // Radius to detect the player

    Transform player; // Reference to the player
    private bool isAIAndPathfindingActive;
    private bool isMovementActive;
    private bool isPlayerInRange;

    void Awake()
    {
        // Get the animator component
        animator = GetComponent<Animator>();

        // Initially, disable all scripts
        if (enemyAI != null) enemyAI.enabled = false;
        if (enemyPathFinding != null) enemyPathFinding.enabled = false;
        if (enemyMovement != null) enemyMovement.enabled = false;

        // Set initial states
        isAIAndPathfindingActive = false;
        isMovementActive = false;
        isPlayerInRange = false;
    }

    void Start()
    {
        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }

        PlayIdleAnimation();
    }

    void Update()
    {
        // Check if the player is within the detection radius
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                StartCoroutine(ToggleStates());
            }
        }
        else
        {
            isPlayerInRange = false;
            StopAllCoroutines(); // Ensure all coroutines are stopped
            PlayIdleAnimation();
        }
    }

    IEnumerator ToggleStates()
    {
        while (isPlayerInRange)
        {
            // Toggle EnemyAI and EnemyPathfinding
            isAIAndPathfindingActive = !isAIAndPathfindingActive;
            SetAIAndPathfinding(isAIAndPathfindingActive);

            // Toggle EnemyMovement based on the state of EnemyAI and EnemyPathfinding
            isMovementActive = !isAIAndPathfindingActive;
            SetMovement(isMovementActive);

            // Play corresponding animations
            if (isAIAndPathfindingActive)
            {
                PlayCleaveAnimation();
            }
            else if (isMovementActive)
            {
                PlayWalkAnimation();
            }

            // Wait for 10 seconds before toggling again
            yield return new WaitForSeconds(8f);
        }
    }

    void SetAIAndPathfinding(bool isActive)
    {
        if (enemyAI != null) enemyAI.enabled = isActive;
        if (enemyPathFinding != null) enemyPathFinding.enabled = isActive;
    }

    void SetMovement(bool isActive)
    {
        if (enemyMovement != null) enemyMovement.enabled = isActive;
    }

    // Function to play the cleave animation
    public void PlayCleaveAnimation()
    {
        animator.Play("Boss_demon_cleave");
    }

    // Function to play the death animation
    public void PlayDeathAnimation()
    {
        animator.Play("Boss_demon_death");
    }

    // Function to play the idle animation
    public void PlayIdleAnimation()
    {
        animator.Play("Boss_demon_idle");
    }

    // Function to play the take damage animation
    public void PlayTakeDamageAnimation()
    {
        animator.Play("Boss_demon_take_damage");
    }

    // Function to play the walk animation
    public void PlayWalkAnimation()
    {
        animator.Play("Boss_demon_walk");
    }

    // Function to draw the detection radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}