using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    void Start()
    {
        MainMenuPanel.SetActive(true);
        LevelSelectorPanel.SetActive(false);
    }
}

