using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opstacle : MonoBehaviour
{

    void Awake()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Events.PlayerDead();    // S�time m�ngija surnuks. 
        }
    }

}
