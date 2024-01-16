using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public float Volume;
    
    private List<Resolution> Resolutions = new List<Resolution>();

    private CanvasScript canvas;

    private void Start()
    {
        canvas = transform.parent.GetComponent<CanvasScript>();
        Resolution[] tempResolutions = Screen.resolutions;
        TMP_Dropdown resolutionDropdown = gameObject.GetComponentInChildren<TMP_Dropdown>();
        resolutionDropdown.ClearOptions();
        List<string> resolutionList = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < tempResolutions.Length; i++)
        {
            if (tempResolutions[i].refreshRateRatio.value != Screen.currentResolution.refreshRateRatio.value)
              continue;
            resolutionList.Add(tempResolutions[i].width + " x " + tempResolutions[i].height);
            Resolutions.Add(tempResolutions[i]);
            if (tempResolutions[i].width == Screen.currentResolution.width && tempResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
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
        canvas.ClickSound.Play(Volume);
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
        canvas.ClickSound.Play(Volume);
        Screen.fullScreen = fullscreen;
    }

    public void SetResolution(int index)
    {
        canvas.ClickSound.Play(Volume);
        Resolution resolution = Resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
