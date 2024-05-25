using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject activeWeapon;
    public GameObject gameoverScreen;
    public GameObject levelUpScreen;
    public GameObject levelCompleteScreen;


    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        LevelUp, 
        LevelComplete
    }
    public GameState currentState;
    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("UI")]
    public GameObject pauseMenu;
    [Header("Current Stats")]
    public Text currentHealthText;
    public Text currentMagnetText;
    public Text currentMoveSpeedText;
    public Text currentRecoveryText;
    public Text currentProjectileSpeedText;
    public Text currentMightText;
    public bool isGameOver = false;
    public bool choosingUpgrade = false;
    public GameObject playerObject;

    [Header("Level Complete Display")]
    public Image characterIcon;
    public Text characterName;
    public List<Image> chosenWeaponsUI = new List<Image>(4);
    public List<Image> chosenPassiveItemsUI = new List<Image>(4);


    void Update()
    {
        switch (currentState)
        {
            case GameState.Playing:
                // Code for playing state
                CheckForPauseAndResume();
                break;
            case GameState.Paused:
                // Code for paused state
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                isGameOver = true;
                Time.timeScale = 0;
                Debug.Log("Game Over");
                DisplayResultScreen();
                break;
            case GameState.LevelComplete:
                LevelComplete();
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0;
                    levelUpScreen.SetActive(true);
                    Debug.Log("Level Up");
                }
                break;
            default:
                Debug.LogError("Invalid game state");
                break;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra " + this + " Deleted");

            Destroy(this);
        }
        currentState = GameState.Playing;
        previousState = GameState.Playing;
        Time.timeScale = 1;
        DisableScreen();
        PlayfabLoginManager playfabLoginManager = FindObjectOfType<PlayfabLoginManager>();
    }
    public void ChangeState(GameState newState)
    {
        previousState = currentState;
        currentState = newState;
    }
    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            // activeWeapon.SetActive(false);
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            // activeWeapon.SetActive(true);
            // activeWeapon.SetActive(true);
            Debug.Log("Game Resumed");
        }
    }
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }
    void DisableScreen()
    {
        pauseMenu.SetActive(false);
        gameoverScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
    }
    public void GameOver()
    {
        ChangeState(GameState.GameOver);
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
    void DisplayResultScreen()
    {
        gameoverScreen.SetActive(true);
    }

    void LevelComplete()
    {
        ChangeState(GameState.LevelComplete);
        Time.timeScale = 0;
        Debug.Log("Level Complete");
        levelCompleteScreen.SetActive(true);
    }
    public void AssignChosenCharacterUI(CharacterScriptableObject character)
    {
        characterIcon.sprite = character.CharacterIcon;
        characterName.text = character.Name;
    }
    public void AssignChosenWeaponsAndPassiveItems(List<Image> weapons, List<Image> passiveItems)
    {
        if (weapons.Count != chosenWeaponsUI.Count || passiveItems.Count != chosenPassiveItemsUI.Count)
        {
            Debug.LogError("UI lists are not the same size as the weapons and passive items lists");
            return;
        }
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = weapons[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }
        for (int i = 0; i < passiveItems.Count; i++)
        {
            if (passiveItems[i] != null)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = passiveItems[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgradeOptions");
        // activeWeapon.SetActive(false);
    }
    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        // activeWeapon.SetActive(true);
        // activeWeapon.SetActive(true);
        // activeWeapon.SetActive(true);
        // activeWeapon.SetActive(true);
        ChangeState(GameState.Playing);

    }
    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if (!instance.damageTextCanvas) return;
        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;
        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }

    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration, float speed)
    {
        // Check if the target transform is null
        if (target == null)
        {
            Debug.LogError("Target transform is null.");
            yield break; // Exit the coroutine if the target transform is null
        }

        // Clone the initial position of the target transform
        Vector3 initialPosition = target.position;

        GameObject textObject = new GameObject("Floating Text");
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        TMP_Text textComponent = textObject.AddComponent<TextMeshProUGUI>(); // Use TextMeshProUGUI instead of Text
        textComponent.text = text;
        textComponent.alignment = TextAlignmentOptions.Center; // Use TextAlignmentOptions instead of TextAnchor
        textComponent.fontSize = textFontSize;
        textComponent.font = textFont; // Assign TMP_FontAsset

        // Use the cloned initial position instead of directly accessing the target transform's position
        rectTransform.position = Camera.main.WorldToScreenPoint(initialPosition);
        Destroy(textObject, duration);
        textObject.transform.SetParent(damageTextCanvas.transform);

        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        Color textColor = textComponent.color;
        while (t < duration)
        {
            yield return wait;
            t += Time.deltaTime;
            textColor.a = 1 - t / duration;
            textComponent.color = textColor;
            yOffset += Time.deltaTime * speed;

            // Check if the target transform is still valid
            if (target != null)
            {
                if (textObject != null)
                {


                    // Use the cloned initial position instead of directly accessing the target transform's position
                    rectTransform.position = Camera.main.WorldToScreenPoint(initialPosition + Vector3.up * yOffset);
                }
            }
        }
    }



}
