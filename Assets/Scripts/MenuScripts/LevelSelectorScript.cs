using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorScript : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public Button MainMenuButton;
    public Button StartLevelButton;
    public Button NextLevelButton;
    public Button PreviousLevelButton;
    public Image LevelImage;
    public TextMeshProUGUI LevelName;
    public TextMeshProUGUI BestLevelTime;

    public List<LevelData> levelDataList = new List<LevelData>();
    private LevelData currentLevelData;
    private int currentLevelIndex;

    public static Action<LevelData> StartLevel;

    void Start()
    {
        MainMenuButton.onClick.AddListener(onMainMenu);
        StartLevelButton.onClick.AddListener(onStartLevel);
        NextLevelButton.onClick.AddListener(onNextLevel);
        PreviousLevelButton.onClick.AddListener(onPreviousLevel);

        PreviousLevelButton.gameObject.SetActive(false);
        if (levelDataList.Count <= 1)
            NextLevelButton.gameObject.SetActive(false);
        else
            NextLevelButton.gameObject.SetActive(true);

        currentLevelIndex = 0;
        if (levelDataList.Count > 0 )
            currentLevelData = levelDataList[0];
        else
            currentLevelData = null;

        if (currentLevelData != null)
            setLevelInfo(currentLevelData);
    }

    public void onMainMenu()
    {
        MainMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onStartLevel()
    {
        StartLevel?.Invoke(currentLevelData);
    }

    public void onNextLevel()
    {
        int levelIndex = currentLevelIndex + 1;
        currentLevelIndex++;
        currentLevelData = levelDataList[levelIndex];
        setLevelInfo(currentLevelData);
        if (levelIndex == levelDataList.Count - 1)
            NextLevelButton.gameObject.SetActive(false);
        if (levelIndex == 1)
            PreviousLevelButton.gameObject.SetActive(true);
    }

    public void onPreviousLevel()
    {
        int levelIndex = currentLevelIndex - 1;
        currentLevelIndex--;
        currentLevelData = levelDataList[levelIndex];
        setLevelInfo(currentLevelData);
        if (levelIndex == 0)
            PreviousLevelButton.gameObject.SetActive(false);
        if (levelIndex == levelDataList.Count - 2) 
            NextLevelButton.gameObject.SetActive(true);
    }

    public void setLevelInfo(LevelData data)
    {
        LevelName.text = data.LevelName;
        LevelImage.sprite = data.IconSprite;
    }
}