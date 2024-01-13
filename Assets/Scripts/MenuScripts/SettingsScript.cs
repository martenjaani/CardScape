using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject MainMenuPanel;
    
    private float Volume;

    private void OnEnable()
    {
        Volume = PlayerPrefs.GetFloat("MasterVolume");
        gameObject.GetComponentInChildren<Slider>().value = Volume;
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
    }

    public void onMainMenu()
    {
        SaveInfo();
        MainMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetFloat("MasterVolume", Volume);
    }
}
