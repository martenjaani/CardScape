using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float accelerationSpeed = 1f;
    public float decelartionSpeed = 1f;
    public float maxSpeed = 1f;
    public float gravityMultiplier = 1f;
    public float jumpPower = 1f;

    public float dashSpeed = 10f;
    public float dashDuration = 1.5f; // Duration of the dash in seconds
    private bool isDashing = false;

    public float ultraDashSpeed = 20f;
    private bool isUltraDashing = false;

    private bool activatedDoubleJump; //Kontrollimaks et kas double jump tehti v�i mitte.
    public bool ActivatedDoubleJump
    {
        get { return activatedDoubleJump; }
        set { activatedDoubleJump = value; }
    }

    private bool movementDisabled = false;
    private bool isDead = false;
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
        ActivatedDoubleJump = false;

        Events.DoubleJumpCardActivated += jump; //EventListener, et kui Event scriptis DoubleJumpCardActivated invokitakse, siis ta h�ppaks
        Events.DashCardActivated += dash;
        Events.OnPlayerDead += setDead;
        Events.UltraDashCardActivated += ultraDash;
        Events.OnGetMovementDisabled += getMovementDisabled;
        Events.OnGetPlayerOnGround += onGround;
        Events.OnSetMovementDisabled += setMovementDisabled;
    }

    private void OnDestroy()
    {
        Events.DoubleJumpCardActivated -= jump;
        Events.DashCardActivated -= dash;
        Events.OnPlayerDead -= setDead;
        Events.UltraDashCardActivated -= ultraDash;
        Events.OnGetMovementDisabled -= getMovementDisabled;
        Events.OnGetPlayerOnGround -= onGround;
        Events.OnSetMovementDisabled -= setMovementDisabled;
    }

    void Update()
    {

        horizontalMovement = getMovementInput();

        movementLogic();                // Liikumise loogika siin sees
        rb.velocity = new Vector2(speed, rb.velocity.y);        // Liigutame uute asukohta

        checkDeath();       // Igal kaadril vaatame kas tegelane on surnud ja maas.

        jumpLogic();

        dashLogic();

        ultraDashLogic();

        switchSides();              // Muudame liikumise suunda kui tarvis.

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, facingRight ? 0f : 180f, 0f));       // P��rame �mber vastavalt facingRight booleanile.
    }

    private void FixedUpdate()
    {
        if (timerStarted)
        {
            if (Time.time > timeEnd)
            {
                timerStarted = false;
                ultraDashCancel();
            }
        }
        if (isUltraDashing & !timerStarted)
        {
            timerStarted = true;
            timeStart = Time.time;
            previousPosition = transform.position;
            timeEnd = timeStart + 0.1f;

        }
    }

    private bool getJumpButtonDown()    // KASUTAME SEDA ET TEADA SAADA KAS ON VAJUTATUD JUMP
    {
        if (getMovementDisabled())
        {
            return false;
        }
        return Input.GetButtonDown("Jump");
    }

    private bool getJumpButtonUp()
    {
        if (movementDisabled)
        {
            return false;
        }
        return Input.GetButtonUp("Jump");
    }

    private float getMovementInput()
    {
        if (getMovementDisabled())
        {
            return 0f;
        }
        return Input.GetAxisRaw("Horizontal");
    }


    private void jumpLogic()
    {
        if (getJumpButtonDown() && onGround())    // Kui vajutad h�ppamist ja oled maas, siis h�ppad.
        {
            jump(false);
        }
        if (getJumpButtonUp() && rb.velocity.y > 0f)     // Kui lased varem lahti on h�pe n�rgem.  Saaks teha ka gravitatsiooni muutmisega, mis on pehmem(?) pmst.
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


        if (rb.velocity.y == 0f && !isDashing && !isUltraDashing)         // Kui oled maa peal. siis tagasi �igele gravitatsioonile.
        {
            rb.gravityScale = gravityMultiplier;
            animator.SetBool("isFalling", false);
        }
        if (rb.velocity.y < 0f && !isDashing && !isUltraDashing)             // Kui hakkad h�ppel kukkuma, on gravitatsioon suurem.
        {
            rb.gravityScale = gravityMultiplier * 1.5f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
    }

    private void jump(bool cardActivation)
    {
        if (cardActivation)
        {
            rb.gravityScale = gravityMultiplier;    // Sätime double jumpiks gravitatsiooni tavaliseks tagasi

            ActivatedDoubleJump = true;
            StartCoroutine(CheckUntilPlayerOnGroundAfterDoubleJump()); //Alustab Coroutine'i p�rast double jump aktiveerimist.
        } 

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        animator.SetBool("isJumping", true);
    }

    private void dash(bool cardActivation)
    {
        if (cardActivation && !isDashing)
        {
            isDashing = true;
            dashSetup();
            CancelInvoke("StopDashing");
        }
    }
    private void ultraDash(bool cardActivation)
    {
        if (cardActivation && !isUltraDashing)
        {
            movementDisabled = true;
            isUltraDashing = true;
            dashSetup();
        }
    }

    private void dashSetup()    // Kui dash algab, on neid vaja teha 1 kord.
    {
        animator.SetBool("isDashing", true);
        animator.SetBool("isJumping", false);   // Kui dash algab, siis tehniliselt enam ei h�ppa
    }

    private void dashLogic()
    {
        if (isDashing)
        {
            if (facingRight) rb.velocity = new Vector2(dashSpeed, 0); // Adjust the direction of dash as per your requirement
            else rb.velocity = new Vector2(-dashSpeed, 0);
            rb.gravityScale = 0;
            horizontalMovement = 0;
            Invoke("StopDashing", dashDuration);
        }
        
    }
    private void StopDashing()
    {
        isDashing = false;
        animator.SetBool("isDashing", false);

        rb.gravityScale = gravityMultiplier;
    }

    private void ultraDashLogic()
    {
        if (isUltraDashing)
        {
            if (facingRight)
            {
                rb.velocity = new Vector2(ultraDashSpeed, 0); // Adjust the direction of dash as per your requirement
                rb.gravityScale = 0;
                ultraDashCancel(facingRight);       // boolean on siin true
            }
            else
            {
                rb.velocity = new Vector2(-ultraDashSpeed, 0);
                rb.gravityScale = 0;
                ultraDashCancel(facingRight);       // siin false
            }
        } 
    }

    private void ultraDashCancel(bool direction)
    {
        if (checkWall(direction))  //kui player model enam ei liigu, siis lopetab dashi
        {
            movementDisabled = false;
            isUltraDashing = false;
            animator.SetBool("isDashing", false);

            rb.velocity = Vector3.zero; // Stop the player when the dash is complete
            rb.gravityScale = gravityMultiplier;
        }
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

    private bool checkWall(bool movingRight)
    {
        if (movingRight)
        {
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.right, 0.1f, Ground);
        } 
        else
        {
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.left, 0.1f, Ground);
        }
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

    public IEnumerator CheckUntilPlayerOnGroundAfterDoubleJump() //P�rast double jumpi aktiveerib selle ning loopib kuni uuesti maas ning ss saab uuesti hiljem double jump'ida
    {
        while (ActivatedDoubleJump)
        {
            ActivatedDoubleJump = !onGround();
            yield return null;
        }
    }

    private void setDead()
    {
        rb.velocity = new Vector2(0,0);    // Saab sellega m�ngida et deathi m�nusamaks teha or something

        isDashing = false;
        isUltraDashing = false; // no more dash kui surnud NO MORE
        animator.SetBool("isDashing", false);   // Sätime dashing asjad falseks

        setMovementDisabled(true);

        isDead = true;
    }
    private void checkDeath()
    {
        if (isDead && onGround())
        {
            isDead = false;
            animator.SetTrigger("Dead");
        }
    }
    private void checkDeath()
    {
        if (isDead && onGround())
        {
            isDead = false;
            animator.SetTrigger("Dead");
        }
    }


    private bool getMovementDisabled()
    {
        return movementDisabled;
    }

    private void setMovementDisabled(bool disable)
    {
        movementDisabled = disable;
    }

    public void RestartLevelOnDeath()   // Selle kutsub death animation v�lja kui l�bi saab
    {
        Events.RestartLevel();
    }
}
