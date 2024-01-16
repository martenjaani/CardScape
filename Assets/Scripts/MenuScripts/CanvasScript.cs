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
    public AudioClipGroup ClickSound;
    public Animator transition;
    public float transitionTime = 1f;


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
        StartCoroutine(onStartLevelCoroutine(data));       
    }

    IEnumerator onStartLevelCoroutine(LevelData data)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(data.SceneName);
    }

    public void onSceneLoaded()
    {
        transition.SetTrigger("End");
        //transition.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
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

