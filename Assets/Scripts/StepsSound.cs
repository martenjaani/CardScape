using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSound : MonoBehaviour
{
    public PlayerMovement player;
    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponent<Animator>();   
        audioSource = this.GetComponent<AudioSource>();
        rb = player.GetComponent<Rigidbody2D>();
        audioSource.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            if (animator.GetBool("isRunning") && rb.velocity.y == 0  && !animator.GetBool("isDashing")) {
                audioSource.mute = false;
            }
            else audioSource.mute = true;
        }
    }
}
