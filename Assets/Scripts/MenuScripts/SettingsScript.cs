using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject MainMenuPanel;

    public static Action<float> setVolume;

    public void SetVolume(float volume)
    {
        setVolume?.Invoke(volume);
    }

    public void onMainMenu()
    {
        MainMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
