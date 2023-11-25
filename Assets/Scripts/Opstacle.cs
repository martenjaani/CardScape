using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Events.PlayerDead();
        }
    }
}
