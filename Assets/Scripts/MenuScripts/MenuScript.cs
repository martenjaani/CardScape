using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject LevelSelectorPanel;
    public GameObject SettingsMenuPanel;

    private CanvasScript canvas;
    private Settings settings;

    private void Start()
    {
        canvas = transform.parent.GetComponent<CanvasScript>();
        settings = SettingsMenuPanel.GetComponent<Settings>();
        settings.gameObject.SetActive(false);
        LevelSelectorPanel.SetActive(false);
    }

    public void onLevelSelectorButton()
    {
        LevelSelectorPanel.SetActive(true);
        gameObject.SetActive(false);
        canvas.ClickSound.Play(settings.Volume);
    }

    public void onSettingsButton()
    {
        SettingsMenuPanel.SetActive(true);
        gameObject.SetActive(false);
        canvas.ClickSound.Play(settings.Volume);
    }

    public void onQuitButton()
    {
        canvas.ClickSound.Play(settings.Volume);
        Application.Quit();
    }
}
