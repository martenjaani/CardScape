using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private float timerTime;
    private bool timerStarted;
    public TextMeshProUGUI EndTimeText;
    public TextMeshProUGUI BestTimeText;

    public GameObject PausePanel;
    public GameObject FinishPanel;

    public AudioClipGroup Dash;
    public AudioClipGroup Landing;
    public AudioClipGroup Death;
    public AudioClipGroup Jump;
    public AudioClipGroup Complete;
    public AudioClipGroup Projectile;

    public static Action sceneLoaded;

    private void Awake()
    {
        sceneLoaded?.Invoke();
        Events.OnRestartLevel += Restart;
        Events.OnFinishLevel += Finish;
        Events.OnPLaySound += PlayCardSound;
        
    }

    private void OnDestroy()
    {
        Events.OnRestartLevel -= Restart;
        Events.OnFinishLevel -= Finish;
    }

    void Start()
    {
        startTime = Time.time;
        timerStarted = true;
        FinishPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    void Update()
    {
        if (timerStarted)
        {
            timerTime = Time.time - startTime;
            timerText.text = FloatTimeToString(timerTime);
        }

        if (Input.GetKeyDown("r"))
            Restart();
        if (Input.GetKeyDown(KeyCode.Escape) && !FinishPanel.activeInHierarchy)
        {
            if (PausePanel.activeInHierarchy)
                onContinue();
            else
                Pause();
        }
    }

    void PlayCardSound(string card)
    {
        switch (card)
        {
            case "Dash":
                Dash.Play(); break;
            case "Landing":
                Landing.Play();break;
            case "Death":
                Death.Play(); break;
            case "Jump":
                Jump.Play(); break;
            case "Complete":
                Complete.Play(); break;
            case "Projectile":
                Projectile.Play(); break;

        }
    }

    private void Restart()  // Läbi death animationi tuleme siia.
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Finish()
    {
        timerStarted = false;
        Events.SetMovementDisabled(true);
        timerText.text = "";

        float currentTime = timerTime;
        EndTimeText.text = "Time: " + FloatTimeToString(currentTime);

        string sceneName = SceneManager.GetActiveScene().name;
        float bestTime = PlayerPrefs.GetFloat(sceneName + "BestTime");
        if (bestTime == 0)
            bestTime = float.MaxValue;
        if(currentTime < bestTime)
        {
            PlayerPrefs.SetFloat(sceneName + "BestTime", currentTime);
            bestTime = currentTime;
        }
        BestTimeText.text = "Best Time: " + FloatTimeToString(bestTime);

        FinishPanel.SetActive(true);
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Events.SetMovementDisabled(true);
        Time.timeScale = 0;
    }

    public void onContinue()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        Events.SetMovementDisabled(false);
    }

    public void onNextLevel()
    {
        Time.timeScale = 1;
        Events.NextLevel();
    }

    public void OnRestartButtonClick()
    {
        Restart();
    }

    public void onQuit()
    {
        Time.timeScale = 1;
        Events.BackToLevelSelector();
        SceneManager.LoadScene("Menu");
    }

    private string FloatTimeToString(float time)
    {
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");
        return minutes + ":" + seconds;
    }
}
