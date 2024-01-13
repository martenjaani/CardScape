using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject MainMenuPanel;
    
    private float Volume;

    public static Action<float> setVolume;

    public void SetVolume(float volume)
    {
        Volume = volume;
        setVolume?.Invoke(Volume);
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
