using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint Next;


    public Waypoint GetNextWaypoint()
    {
        return Next;

    }

    private void OnDrawGizmos()
    {
        if (Next != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, Next.transform.position);
        }
        
    }
}