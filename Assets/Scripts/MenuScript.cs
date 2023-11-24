using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject LevelSelectorPanel;
    public Button levelSelectorButton;
    public Button settingsButton;
    public Button quitButton;


    void Start()
    {
        levelSelectorButton.onClick.AddListener(onLevelSelectorButton);
        settingsButton.onClick.AddListener(onSettingsButton);
        quitButton.onClick.AddListener(onQuitButton);
    }

    public void onLevelSelectorButton()
    {
        LevelSelectorPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onSettingsButton()
    {

    }

    public void onQuitButton()
    {
        Application.Quit();
    }
}
