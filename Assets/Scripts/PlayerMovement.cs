using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float accelerationSpeed = 1f;
    public float decelartionSpeed = 1f;
    public float maxSpeed = 1f;
    public float gravityMultiplier = 1f;
    public float jumpPower = 1f;
    public float jumpPowerMultiplierOnCardActivation = 2f;

    private float horizontalMovement;
    private bool facingRight = true;
    private float speed;

    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Animator animator;
    [SerializeField] private LayerMask Ground;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        rb.gravityScale = gravityMultiplier;

        Events.DoubleJumpCardActivated += jump; //EventListener, et kui Event scriptis DoubleJumpCardActivated invokitakse, siis ta h�ppaks
    }

    private void OnDestroy()
    {
        Events.DoubleJumpCardActivated -= jump;
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");    
        movementLogic();                // Liikumise loogika siin sees
        rb.velocity = new Vector2(speed, rb.velocity.y);        // Liigutame uute asukohta

        jumpLogic();        // H�ppamise loogika siin sees

        switchSides();              // Muudame liikumise suunda kui tarvis.
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, facingRight ? 0f : 180f, 0f));       // P��rame �mber vastavalt facingRight booleanile.
    }

    private void jumpLogic()
    {
        if (Input.GetButtonDown("Jump") && onGround())    // Kui vajutad h�ppamist ja oled maas, siis h�ppad.
        {
            jump(false);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)     // Kui lased varem lahti on h�pe n�rgem.  Saaks teha ka gravitatsiooni muutmisega, mis on pehmem(?) pmst.
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


        if (rb.velocity.y < 0f)                          // Kui hakkad h�ppel kukkuma, on gravitatsioon suurem.
        {
            rb.gravityScale = gravityMultiplier * 1.5f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        if (rb.velocity.y == 0f)                       // Kui oled maa peal. siis tagasi �igele gravitatsioonile.
        {
            rb.gravityScale = gravityMultiplier;
            animator.SetBool("isFalling", false);
        }
    }

    private void jump(bool cardActivation)
    {
        if (cardActivation)
            jumpPower *= jumpPowerMultiplierOnCardActivation; //Jumplogic kasutab seda et kas space hoitakse all v�i mitte, mis siis on vaja jumppowerit t�sta et ta k�rgemale h�ppaks
                                                              //kaardi aktiveerimisel

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        animator.SetBool("isJumping", true);

        if (cardActivation)
            jumpPower /= jumpPowerMultiplierOnCardActivation;
    }

    private void movementLogic()     // Movement loogika
    {
        if (horizontalMovement > 0) // Input Managerist saame teada, kas liigutakse paremale v�i vasakule. Tagastab kas -1, 0 v�i 1. Seej�rel lisame kiirenduse v��rtuse
        {
            speed = Mathf.Clamp(rb.velocity.x + (accelerationSpeed * Time.deltaTime), -maxSpeed, maxSpeed);
            animator.SetBool("isRunning", true);    // Paneme animation k�ima
        }
        else if (horizontalMovement < 0)
        {
            speed = Mathf.Clamp(rb.velocity.x - (accelerationSpeed * Time.deltaTime), -maxSpeed, maxSpeed);     // Vasakule liikumise puhul lahutame.
            animator.SetBool("isRunning", true);
        }
        else
        {
            if (rb.velocity.x > 0.05f)           // Siin on aeglustamise loogika.
            {
                speed = Mathf.Clamp(rb.velocity.x - (decelartionSpeed * Time.deltaTime), -maxSpeed, maxSpeed); // Vastavalt liikumise suunale kas lahutame v�i liidame decelerationspeed muutuja
            }
            else if (rb.velocity.x < -0.05f)
            {
                speed = Mathf.Clamp(rb.velocity.x + (decelartionSpeed * Time.deltaTime), -maxSpeed, maxSpeed);
            }
            else
            {
                speed = 0f;     // Kui kiirus on piisavalt l�hedal nullile, s�time selle nulliks
                animator.SetBool("isRunning", false);
            }
        }
    }

    private bool onGround() // Kontrollib,  kas tegelane on maa peal
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, 0.1f, Ground);
    }

    private void switchSides()
    {
        if (horizontalMovement > 0f)      // Kui vaatab paremale, aga liikumine on vasakule, p��rame �mber
        {
            facingRight = true;
        } 
        else if(horizontalMovement < 0f)  // Kui vaatab vasakule, aga liikumine on paremale, p��rame samuti �mber
        {
            facingRight = false;
        }
    }
}
