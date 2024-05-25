using System.Collections;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    private Death deathScript;
    private EnemyPathfinding pathfindingScript;
    private EnemyAI aiScript;

    void Start()
    {
        deathScript = GetComponent<Death>();
        pathfindingScript = GetComponent<EnemyPathfinding>();
        aiScript = GetComponent<EnemyAI>();

        if (deathScript == null)
        {
            Debug.LogError("Death script not found on " + gameObject.name);
        }

        if (pathfindingScript == null)
        {
            Debug.LogError("EnemyPathfinding script not found on " + gameObject.name);
        }

        if (aiScript == null)
        {
            Debug.LogError("EnemyAI script not found on " + gameObject.name);
        }

        // Start the coroutine to toggle scripts
        StartCoroutine(ToggleScripts());
    }

    IEnumerator ToggleScripts()
    {
        while (true)
        {
            // Deactivate EnemyPathfinding and EnemyAI scripts, activate Death script
            deathScript.enabled = true;
            pathfindingScript.enabled = false;
            aiScript.enabled = false;

            // Wait for 10 seconds
            yield return new WaitForSeconds(10f);

            // Deactivate Death script, activate EnemyPathfinding and EnemyAI scripts
            deathScript.enabled = false;
            pathfindingScript.enabled = true;
            aiScript.enabled = true;

            // Wait for 10 seconds
            yield return new WaitForSeconds(10f);
        }
    }
}
