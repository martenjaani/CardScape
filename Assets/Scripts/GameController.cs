using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText;
    private float startTime;
    private float timerTime;
    private bool timerStarted;
    public TextMeshProUGUI EndTimeText;
    public TextMeshProUGUI BestTimeText;

    public GameObject FinishPanel;
    public GameObject PausePanel;
    public GameObject SettingsPanel;
    public GameObject Countdown;

    [Header("Sound")]
    public float MasterVolume = 10.0f;
    public AudioSource RainSound;
    public AudioSource StepsSound;
    public AudioSource ClickSound;
    private float RainSoundSetVolume;
    private float StepsSoundSetVolume;
    private float ClickSoundSetVolume;
    public AudioClipGroup Dash;
    public AudioClipGroup Landing;
    public AudioClipGroup Death;
    public AudioClipGroup Jump;
    public AudioClipGroup Complete;
    public AudioClipGroup Projectile;
    public AudioClipGroup Grapple;
    

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
        Events.OnPLaySound -= PlayCardSound;
    }

    void Start()
    {
        StartCoroutine(CountDownAnim());
        Events.SetMovementDisabled(true);
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        RainSoundSetVolume = RainSound.volume;
        StepsSoundSetVolume = StepsSound.volume;
        ClickSoundSetVolume = ClickSound.volume;
        SetVolume(MasterVolume);
        //startTime = Time.time;
        //timerStarted = true;
        FinishPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    private IEnumerator CountDownAnim()
    {
        int seconds = Countdown.transform.childCount;
        for (int i = 0; i < seconds; i++) 
        {
            Countdown.transform.GetChild(i).gameObject.SetActive(true);
            if(i == seconds - 1)
                yield return null;
            else
                yield return new WaitForSeconds(1);
        }
        Events.SetMovementDisabled(false);
        startTime = Time.time;
        timerStarted = true;
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
                Dash.Play(MasterVolume); break;
            case "Landing":
                Landing.Play(MasterVolume); break;
            case "Death":
                Death.Play(MasterVolume); break;
            case "Jump":
                Jump.Play(MasterVolume); break;
            case "Complete":
                Complete.Play(MasterVolume); break;
            case "Projectile":  
                Projectile.Play(MasterVolume); break;
            case "Grapple":
                Grapple.Play(MasterVolume); break;
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
        ClickSound.Play();
        PausePanel.SetActive(true);
        Events.SetMovementDisabled(true);
        Time.timeScale = 0;
    }

    public void onFinishToSettings()
    {
        ClickSound.Play();
        FinishPanel.SetActive(false);
        SettingsPanel.GetComponentInChildren<Slider>().value = MasterVolume;
        SettingsPanel.SetActive(true);
    }

    public void onSettingsToFinish()
    {
        ClickSound.Play();
        SettingsPanel.SetActive(false);
        FinishPanel.SetActive(true);
    }

    public void onSettings()
    {
        ClickSound.Play();
        PausePanel.SetActive(false);
        SettingsPanel.GetComponentInChildren<Slider>().value = MasterVolume;
        SettingsPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        MasterVolume = volume;
        RainSound.volume = RainSoundSetVolume * volume;
        StepsSound.volume = StepsSoundSetVolume * volume;
        ClickSound.volume = ClickSoundSetVolume * volume;
    }

    public void onSettingsToPause()
    {
        ClickSound.Play();
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        SettingsPanel.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void onContinue()
    {
        ClickSound.Play();
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        Events.SetMovementDisabled(false);
    }

    public void onNextLevel()
    {   
        ClickSound.Play();
        Time.timeScale = 1;
        FinishPanel.SetActive(false);
        Events.NextLevel();
    }

    public void OnRestartButtonClick()
    {
        ClickSound.Play();
        Restart();
    }

    public void onQuit()
    {
        ClickSound.Play();
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
