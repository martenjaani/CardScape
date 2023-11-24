using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointProjectile : MonoBehaviour
{
    public float Speed = 5;
    public Waypoint Next;

    //public static int EnemyCount=0;

    void Update()
    {
        if (Next != null)
        {
            if (Vector3.Distance(transform.position, Next.transform.position) < 0.1)
            {
                Next = Next.GetNextWaypoint();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Next.transform.position, Time.deltaTime * Speed);
            }
        }
        else
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
