using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    protected PlayerStats playerStats;
    public PassiveItemsScriptableObject passiveItemsScriptableObject;

    protected virtual void ApplyModifiers()
    {
    }

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        ApplyModifiers();
    }
}