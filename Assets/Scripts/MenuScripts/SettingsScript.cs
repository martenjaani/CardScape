using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject MainMenuPanel;
    
    private float Volume;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        TMP_Dropdown resolutionDropdown = gameObject.GetComponentInChildren<TMP_Dropdown>();
        resolutionDropdown.ClearOptions();
        List<string> resolutionList = new List<string>();
        int currentResolutionIndex = 0;
        int counter = 0;
        foreach (Resolution resolution in resolutions)
        {
            resolutionList.Add(resolution.width + " x " + resolution.height);
            if(resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = counter;
            }
            counter++;
        }
        resolutionDropdown.AddOptions(resolutionList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

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

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
