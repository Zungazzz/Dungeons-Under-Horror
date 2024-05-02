using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public void NextCharacter()
    {
        if (characters[selectedCharacter] != null)
        {
            characters[selectedCharacter].SetActive(false);
        }
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        if (characters[selectedCharacter] != null)
        {
            characters[selectedCharacter].SetActive(true);
        }
    }

    public void PreviousCharacter()
    {
        if (characters[selectedCharacter] != null)
        {
            characters[selectedCharacter].SetActive(false);
        }
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter = characters.Length - 1;
        }
        if (characters[selectedCharacter] != null)
        {
            characters[selectedCharacter].SetActive(true);
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
