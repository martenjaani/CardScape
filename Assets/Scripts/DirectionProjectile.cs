using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionProjectile : MonoBehaviour
{
    public float Speed = 5;
    public Vector3 direction;
    public float lifeTime;
    private float endTime;


    private void Start()
    {
        endTime = Time.time + lifeTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (direction != null)
        {

            transform.position += direction * Time.deltaTime * Speed;

        }
        else
        {
            Destroy(gameObject);
        }
        if (endTime < Time.time)
        {
            Destroy(gameObject);
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
