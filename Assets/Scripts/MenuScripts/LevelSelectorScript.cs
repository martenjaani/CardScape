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
    public GameObject LevelImagePrefab;
    
    private PositionAnimation ToRightAnimation;
    private PositionAnimation ToLeftAnimation;

    public List<LevelData> levelDataList = new List<LevelData>();
    private LevelData currentLevelData;
    private int currentLevelIndex;
    private bool nextLevelCalled = false;

    public static Action<LevelData> StartLevel;

    void Start()
    {
        Events.nextLevel += NextLevelCalled;
        Events.backToLevelSelector += BackToLevelSelectorCalled;

        SetPostitionScripts(LevelImage.gameObject);

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
        currentLevelData = levelDataList[currentLevelIndex];
        setLevelInfo(currentLevelData);
        LevelImage.sprite = currentLevelData.IconSprite;
    }

    private void SetPostitionScripts(GameObject LevelImageObject)
    {
        PositionAnimation[] positionAnimations = LevelImageObject.GetComponentsInChildren<PositionAnimation>();
        ToRightAnimation = positionAnimations[0];
        ToLeftAnimation = positionAnimations[1];
        ToRightAnimation.ToRight = true;
        ToLeftAnimation.ToRight = false;
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
        ToLeftAnimation.enabled = true;
        ToLeftAnimation.DestroyOnEnd = true;

        int levelIndex = currentLevelIndex + 1;
        currentLevelIndex++;
        currentLevelData = levelDataList[levelIndex];

        GameObject levelImageObject = Instantiate(LevelImagePrefab, transform);
        LevelImage = levelImageObject.GetComponent<Image>();
        LevelImage.sprite = currentLevelData.IconSprite;
        levelImageObject.transform.localPosition = new Vector3(800, 0);
        SetPostitionScripts(levelImageObject);
        ToLeftAnimation.enabled = true;
        
        setLevelInfo(currentLevelData);
        
        if (levelIndex == levelDataList.Count - 1)
            NextLevelButton.gameObject.SetActive(false);
        if (levelIndex == 1)
            PreviousLevelButton.gameObject.SetActive(true);
        if (nextLevelCalled)
        {
            onStartLevel();
            nextLevelCalled = false;
        }
    }

    public void NextLevelCalled()
    {
        nextLevelCalled = true;
        onNextLevel();
    }

    public void onPreviousLevel()
    {
        ToRightAnimation.enabled = true;
        ToRightAnimation.DestroyOnEnd = true;
        
        int levelIndex = currentLevelIndex - 1;
        currentLevelIndex--;
        currentLevelData = levelDataList[levelIndex];

        GameObject levelImageObject = Instantiate(LevelImagePrefab, transform);
        LevelImage = levelImageObject.GetComponent<Image>();
        LevelImage.sprite = currentLevelData.IconSprite;
        levelImageObject.transform.localPosition = new Vector3(-800, 0);
        SetPostitionScripts(levelImageObject);
        ToRightAnimation.enabled = true;

        setLevelInfo(currentLevelData);
        if (levelIndex == 0)
            PreviousLevelButton.gameObject.SetActive(false);
        if (levelIndex == levelDataList.Count - 2) 
            NextLevelButton.gameObject.SetActive(true);
    }

    private void BackToLevelSelectorCalled()
    {
        setLevelInfo(currentLevelData);
    }

    public void setLevelInfo(LevelData data)
    {
        LevelName.text = data.LevelName;
        float bestTime = PlayerPrefs.GetFloat(currentLevelData.SceneName + "BestTime");
        if (bestTime == 0)
            BestLevelTime.text = "Unfinished";
        else
        {
            string minutes = ((int)bestTime / 60).ToString();
            string seconds = (bestTime % 60).ToString("f2");
            BestLevelTime.text = "Best Time: " + minutes + ":" + seconds;
        }
    }

    public void DeleteBestTimeData()
    {
        PlayerPrefs.DeleteKey(currentLevelData.SceneName + "BestTime");
        setLevelInfo(currentLevelData);
    }

    private void OnDestroy()
    {
        Events.nextLevel -= NextLevelCalled;
    }
}