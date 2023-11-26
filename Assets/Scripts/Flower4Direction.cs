using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower4Direction : MonoBehaviour
{
    public DirectionProjectile DirectionProjectile;
    public float FireDelay = 1;
    public float NextFireTime;

    public bool up;
    public bool down;
    public bool right;
    public bool left;
    private bool Fireing;
    private Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        NextFireTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {

        if (NextFireTime < Time.time && gameObject.GetComponent<Renderer>().isVisible)
        {
            if (up)
            {
                DirectionProjectile flowerProjectile = Instantiate<DirectionProjectile>(DirectionProjectile);
                flowerProjectile.direction = new Vector3(0, 1, 0);
                spawnPos = transform.position;
                spawnPos.y -= 0.5f;
                flowerProjectile.transform.position = spawnPos;

            }
            if (down)
            {
                DirectionProjectile flowerProjectile = Instantiate<DirectionProjectile>(DirectionProjectile);
                flowerProjectile.direction = new Vector3(0, -1, 0);
                spawnPos = transform.position;
                spawnPos.y -= 0.5f;
                flowerProjectile.transform.position = spawnPos;

            }
            if (right)
            {
                DirectionProjectile flowerProjectile = Instantiate<DirectionProjectile>(DirectionProjectile);
                flowerProjectile.direction = new Vector3(1, 0, 0);
                spawnPos = transform.position;
                spawnPos.y -= 0.5f;
                flowerProjectile.transform.position = spawnPos;

            }
            if (left)
            {
                DirectionProjectile flowerProjectile = Instantiate<DirectionProjectile>(DirectionProjectile);
                flowerProjectile.direction = new Vector3(-1, 0, 0);
                spawnPos = transform.position;
                spawnPos.y -= 0.5f;
                flowerProjectile.transform.position = spawnPos;

            }
            NextFireTime = Time.time + FireDelay;
            Events.PlaySound("Projectile");


        }


    }

}
