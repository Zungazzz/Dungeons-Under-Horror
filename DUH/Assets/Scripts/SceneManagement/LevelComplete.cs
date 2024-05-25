using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {

            GameManager.instance.currentState = GameManager.GameState.LevelComplete;

            // Activate the level complete screen
            GameManager.instance.levelCompleteScreen.SetActive(true);

        }
    }
}

