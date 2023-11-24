using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnemy : MonoBehaviour
{
    public FlowerProjectile flowerProjectilePrefab;
    public float FireDelay = 1;
    public float NextFireTime;
    private PlayerMovement Target;
    private bool Fireing;
    // Start is called before the first frame update
    void Start()
    {
        NextFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Fireing == true)
        {
            if(NextFireTime < Time.time)
            {
                FlowerProjectile flowerProjectile = Instantiate<FlowerProjectile>(flowerProjectilePrefab);
                flowerProjectile.Target = Target;
                flowerProjectile.transform.position = transform.position;
                NextFireTime = Time.time + FireDelay;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Target= player;
            Fireing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Fireing = false ;
        }
    }


}
