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

    void Start()
    {
        MainMenuButton.onClick.AddListener(onMainMenu);
        StartLevelButton.onClick.AddListener(onStartLevel);
        NextLevelButton.onClick.AddListener(onNextLevel);
        PreviousLevelButton.onClick.AddListener(onPreviousLevel);

        NextLevelButton.gameObject.SetActive(false);
        PreviousLevelButton.gameObject.SetActive(false);
        
        if(levelDataList.Count > 0 )
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
        SceneManager.LoadScene(currentLevelData.SceneName);
        gameObject.SetActive(false);
    }

    public void onNextLevel()
    {

    }

    public void onPreviousLevel()
    {

    }

    public void setLevelInfo(LevelData data)
    {
        LevelName.text = data.LevelName;
        LevelImage.sprite = data.IconSprite;
    }
}