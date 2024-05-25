using System.Collections;
using UnityEngine;

public class Death : MonoBehaviour
{
    private Animator animator;
    public GameObject minionPrefab; // Prefab for the minions to summon
    AudioManager audioManager;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        if (minionPrefab == null)
        {
            Debug.LogError("Minion prefab not assigned in the inspector.");
        }

        // Start the summoning coroutine immediately
        StartCoroutine(SummonCoroutine());
    }
    void Awake()
    {
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
    }

    IEnumerator SummonCoroutine()
    {
        while (true)
        {
            // Play the summon animation
            PlaySummonAnimation();

            // Wait for the duration of the summon animation
            yield return new WaitForSeconds(GetAnimationLength("summon"));
            audioManager.PlaySFX(audioManager.Summon);
            // Summon minions at specified positions
            Vector2[] positions = new Vector2[]
            {
                new Vector2(transform.position.x + 2, transform.position.y),
                new Vector2(transform.position.x - 2, transform.position.y),
                new Vector2(transform.position.x, transform.position.y + 2),
                new Vector2(transform.position.x, transform.position.y - 2)
            };
            SummonMinions(positions);

            // Play the idle animation
            PlayIdleAnimation();

            // Wait for 5 seconds before the next summoning sequence
            yield return new WaitForSeconds(5f);
        }
    }

    public void PlaySkill1Animation()
    {
        if (animator != null)
        {
            animator.Play("skill1");
        }
        else
        {
            Debug.LogError("Animator is null, cannot play skill1 animation.");
        }
    }

    public void PlayIdleAnimation()
    {
        if (animator != null)
        {
            animator.Play("idle");
        }
        else
        {
            Debug.LogError("Animator is null, cannot play idle animation.");
        }
    }

    public void PlaySummonAnimation()
    {
        if (animator != null)
        {
            animator.Play("summon");
        }
        else
        {
            Debug.LogError("Animator is null, cannot play summon animation.");
        }
    }

    float GetAnimationLength(string animationName)
    {
        if (animator != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == animationName)
                {
                    return clip.length;
                }
            }
        }
        return 0f;
    }

    // Method to summon minions at specified positions
    public void SummonMinions(Vector2[] positions)
    {
        if (minionPrefab != null)
        {
            foreach (var position in positions)
            {
                Instantiate(minionPrefab, position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Minion prefab is null, cannot summon minions.");
        }
    }
}
