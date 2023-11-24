using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GrapplingRope grappleRope;

    public Camera main_camera;

    public Transform player;
    public Transform firePoint;     // See m‰‰rab laskmise alustuskoha

    public SpringJoint2D springJoint2D;
    public Rigidbody2D rb;

    [SerializeField] private float maxDistance = 20f;

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = false;
    [SerializeField] private float launchSpeed = 1f;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3f;
    [SerializeField] private float targetFrequncy = 1f;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;


    public void SetGrapplePoint()
    {
        Vector2 distanceVector = main_camera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;  // Leiame mis suunda lasta
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized,maxDistance,LayerMask.NameToLayer("Ground")))
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, maxDistance, 8);        // Otsime, kas grapple sai millelegi pihta
            if (true)            // Paneme, et grappbleb ainult leveli objektidele
            {
                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance)
                {
                    grapplePoint = hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)firePoint.position;
                    grappleRope.enabled = true;

                }
            }

        }
    }

    public void Grapple()       // Teostame grapple
    {
        springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            springJoint2D.distance = targetDistance;
            springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                springJoint2D.autoConfigureDistance = true;
                springJoint2D.frequency = 0;
            }

            springJoint2D.connectedAnchor = grapplePoint;
            springJoint2D.enabled = true;
        }
        else
        {
            springJoint2D.connectedAnchor = grapplePoint;

            Vector2 distanceVector = firePoint.position - player.position;

            springJoint2D.distance = distanceVector.magnitude;
            springJoint2D.frequency = launchSpeed;
            springJoint2D.enabled = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }
}
