using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    public float Speed = 5;
    public PlayerMovement Target;


    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {

            transform.position += (Target.transform.position - transform.position).normalized * Time.deltaTime * Speed;

        }
   

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Events.PlayerDead();
        }
    }
}
