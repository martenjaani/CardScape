using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject LevelSelectorPanel;
    public GameObject SettingsMenuPanel;

    public void onLevelSelectorButton()
    {
        LevelSelectorPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onSettingsButton()
    {
        SettingsMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onQuitButton()
    {
        Application.Quit();
    }
}
