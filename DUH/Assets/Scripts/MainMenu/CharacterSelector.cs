using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour

{
    public static CharacterSelector instance;
    public CharacterScriptableObject characterData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static CharacterScriptableObject GetData()
    {
        return instance.characterData;
    }
    public void SelectCharacter(CharacterScriptableObject data)
    {
        characterData = data;
    }
}
