using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] public int timeBetweenStaminaRefresh = 3;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "Stamina";

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;

        // Ensure the stamina container has the correct number of children
        InitializeStaminaUI();

        // Start the stamina refresh routine
        StartCoroutine(RefreshStaminaRoutine());
    }

    private void InitializeStaminaUI()
    {
        // Reset existing stamina UI elements
        for (int i = 0; i < maxStamina; i++)
        {
            if (i < staminaContainer.childCount)
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
            }
        }
    }

    public void UseStamina()
    {
        if (CurrentStamina > 0)
        {
            CurrentStamina--;
            UpdateStaminaImages();
        }
    }

    public void RefreshStamina()
    {
        if (CurrentStamina < maxStamina)
        {
            CurrentStamina++;
            UpdateStaminaImages();
        }
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateStaminaImages()
    {
        Debug.Log("Updating Stamina Images...");
        for (int i = 0; i < maxStamina; i++)
        {
            if (i < staminaContainer.childCount)
            {
                if (i < CurrentStamina)
                {
                    staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
                }
                else
                {
                    staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
                }
            }
        }

        if (CurrentStamina < maxStamina)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }
}
