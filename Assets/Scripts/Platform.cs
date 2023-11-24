using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float Speed;
    public float XDistance;
    public float YDistance;
    public bool loop;
    public bool ToRight;
    public bool MoveWith;

    private Vector3 target;
    private Vector3 PlayerToPlatformOffset;
    private PlayerMovement player = null;

    void Start()
    {
        SetTarget();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);

        if(player != null && MoveWith)
            player.transform.position = transform.position + PlayerToPlatformOffset;
        
        float distancee = Vector3.SqrMagnitude(transform.position - target);
        if(distancee < float.Epsilon)
        {
            if(ToRight == true)
                ToRight = false;
            else
                ToRight = true;

            if (loop == true)
                SetTarget();
            else
                Destroy(this);
        }
    }

    void SetTarget()
    {
        Vector3 position = transform.position;
        if (ToRight)
            target = new Vector3(position.x + XDistance, position.y + YDistance, position.z);
        else
            target = new Vector3(position.x - XDistance, position.y - YDistance, position.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        player = collision.GetComponent<PlayerMovement>();
        if (player != null)
            PlayerToPlatformOffset = player.transform.position - transform.position;
    }
}