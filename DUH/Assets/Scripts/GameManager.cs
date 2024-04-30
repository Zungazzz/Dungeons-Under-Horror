using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public PlayerStats player;

    public int coins;
    public int experience;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (instance == null)
        {
            instance = this;
        }
        experience = player.experience;
    }
    private void Update(){
        experience = player.experience;
    }
    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";
        PlayerPrefs.SetString("Save", s);
    }
    public void LoadState()
    {
        if(!PlayerPrefs.HasKey("Save"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("Save").Split('|');

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);

    }
}
