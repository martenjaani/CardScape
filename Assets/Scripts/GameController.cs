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
    private bool pauseTimer;
    public TextMeshProUGUI EndTimeText;

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
        pauseTimer = false;
    }

    void Update()
    {
        if (timerStarted && !pauseTimer)
        {
            timerTime = Time.time - startTime;

            string minutes = ((int)timerTime / 60).ToString();
            string seconds = (timerTime % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;
        }
        if(pauseTimer)
        {
            startTime = Time.time - timerTime;
        }

        if (Input.GetKeyDown("r"))
            Restart();
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
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
        PausePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Finish()
    {
        timerStarted = false;
        EndTimeText.text = "Time: "+timerText.text;
        FinishPanel.SetActive(true);
        timerText.text = "";
        Events.SetMovementDisabled(true);
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Events.SetMovementDisabled(true);
        pauseTimer = true;
    }

    public void onContinue()
    {
        PausePanel.SetActive(false);
        Events.SetMovementDisabled(false);
        pauseTimer = false;
    }

    public void onNextLevel()
    {
        Events.NextLevel();
    }

    public void OnRestartButtonClick()
    {
        Restart();
    }

    public void onQuit()
    {
        SceneManager.LoadScene("Menu");
    }
}
