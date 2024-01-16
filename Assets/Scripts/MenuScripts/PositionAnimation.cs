using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAnimation : MonoBehaviour
{
    public AnimationCurve Curve;
    public float Speed;
    public bool ToRight;
    public bool DestroyOnEnd;
    
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private float timeAggregate;

    private void OnEnable()
    {
        timeAggregate = 0;
        StartPosition = transform.localPosition;
        EndPosition = StartPosition;
        if (ToRight)
            EndPosition.x += 800;
        else
            EndPosition.x -= 800;
    }

    private void Update()
    {
        timeAggregate += Time.deltaTime * Speed;
        float value = Curve.Evaluate(timeAggregate);
        transform.localPosition = Vector3.LerpUnclamped(StartPosition, EndPosition, value);
        if(timeAggregate >= 1)
        {
            transform.localPosition = EndPosition;
            enabled = false;
            if(DestroyOnEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}
