using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool timerStarted;
    public TextMeshProUGUI EndTimeText;
    public GameObject FinishPanel;

    public AudioClipGroup Dash;
    public AudioClipGroup Landing;
    public AudioClipGroup Death;
   

    private void Awake()
    {
        Events.OnRestartLevel += Restart;
        Events.OnFinishLevel += Finish;
        Events.OnPLaySound += PlayCardSound;
        
    }

    private void OnDestroy()
    {
        Events.OnRestartLevel -= Restart;
        Events.OnFinishLevel -= Finish;
    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        timerStarted = true;
        FinishPanel.SetActive(false);
         
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("r"))
        {
            Restart();
        }
        if (timerStarted)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;
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
       
        }
    }

    private void Restart()  // Läbi death animationi tuleme siia.
    {
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

    public void OnRestartButtonClick()
    {
        Restart();
    }


}
