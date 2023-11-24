using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript instance;
    public GameObject MainMenuPanel;
    public GameObject LevelSelectorPanel;


    private void Awake()
    {
        if(instance != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LevelSelectorScript.StartLevel += onStartLevel;
    }

    public void onStartLevel(LevelData data)
    {
        GameController.sceneLoaded += onSceneLoaded;
        SceneManager.LoadScene(data.SceneName);       
    }

    public void onSceneLoaded()
    {
        GameController.sceneLoaded -= onSceneLoaded;
        MenuLoaded.menuLoaded += onMenuLoaded;
        LevelSelectorPanel.SetActive(false);
    }

    public void onMenuLoaded()
    {
        MenuLoaded.menuLoaded -= onMenuLoaded;
        LevelSelectorPanel.SetActive(true);
    } 
}

