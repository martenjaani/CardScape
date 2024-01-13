using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject MainMenuPanel;

    public void SetVolume(float volume)
    {

    }

    public void onMainMenu()
    {
        MainMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
