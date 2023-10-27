using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Events.PlayerDead();    // Sätime mängija surnuks. Death animation lõpus level reset
        }
    }

}
