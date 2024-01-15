using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Events.PlaySound("Complete");
            Events.FinishLevel();    // Sätime mängija surnuks. Death animation lõpus level reset
            player.GetComponent<Rigidbody2D>().simulated = false;
           
            player.GetComponent<BoxCollider2D>().enabled = false;
           // player.enabled = false;

        }
    }
}
