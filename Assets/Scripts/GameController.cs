using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private void Awake()
    {
        Events.OnRestartLevel += Restart;
    }

    private void OnDestroy()
    {
        Events.OnRestartLevel -= Restart;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            Restart();
        }
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
