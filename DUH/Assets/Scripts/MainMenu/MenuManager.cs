using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI VolumeNumber;
    private Slider volumeSlider;

    void start()
    {
        volumeSlider = GetComponent<Slider>();
        VolumeChange(volumeSlider.value);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void VolumeChange(float volume)
    {
        VolumeNumber.text = volume.ToString("0");
    }

}
