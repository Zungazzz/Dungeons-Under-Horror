using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItems", menuName = "ScriptableObjects/PassiveItemsScriptableObject", order = 1)]
public class PassiveItemsScriptableObject : ScriptableObject
{
    [SerializeField]
    float multipliers;

    // Public property to access the multipliers
    public float Multiply
    {
        get { return multipliers; }
        set { multipliers = value; }
    }

    [SerializeField]
    int level;
    public int Level
    {
        get => level; private set => level = value;
    }
    [SerializeField]
    GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab
    {
        get => nextLevelPrefab; private set => nextLevelPrefab = value;
    }
       [SerializeField]
    new string name;
    public string Name
    {
        get => name; private set => name = value;
    }
    [SerializeField]
    string description;
    public string Description
    {
        get => description; private set => description = value;
    }
    [SerializeField]
    Sprite icon;
    public Sprite Icon
    {
        get => icon; private set => icon = value;
    }

}