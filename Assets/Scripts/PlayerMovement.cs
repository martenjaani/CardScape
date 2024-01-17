using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float accelerationSpeed = 1f;
    public float decelartionSpeed = 1f;
    public float maxSpeed = 1f;

    [Header("Jump")]
    public float gravityMultiplier = 1f;
    public float jumpPower = 1f;
    private bool isFalling = false;

    [Header("WallJump")]
    public float wallJumpDistance = 1f;
    public float wallJumpPower = 1f;
    private bool ActivatedWallJump = false;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashDuration = 1.5f; // Duration of the dash in seconds
    private bool isDashing = false;

    [Header("Ultradash")]
    public float ultraDashSpeed = 20f;
    private bool isUltraDashing = false;

    [Header("Wallslide")]
    public float wallSlideSpeed = 2f;
    public bool isWallSliding = false;

    [Header("Grapple")]
    //public GrapplingRope grappleRope;
    public Camera main_camera;
    public Transform firePoint;     // See määrab laskmise alustuskoha
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private bool launchToPoint = false;
    [SerializeField] private float launchSpeed = 1f;
    [SerializeField] private float hopStrength = 1f;

    [HideInInspector] public Vector2 grapplePoint;      // Grapple abi


    private bool activatedDoubleJump; //Kontrollimaks et kas double jump tehti v�i mitte.
    public bool ActivatedDoubleJump
    {
        get { return activatedDoubleJump; }
        set { activatedDoubleJump = value; }
    }

    private bool movementDisabled = false;
    private bool isDead = false;
    private bool isGrappling = false;
    //private bool playerSingleDeath = true;
    private float horizontalMovement;
    private bool facingRight = true;
    private float speed;

    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Animator animator;
    private LineRenderer lineRenderer;

    [Header("Misc")]
    [SerializeField] private LayerMask Ground;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        rb.gravityScale = gravityMultiplier;
        ActivatedDoubleJump = false;

        lineRenderer.enabled = false;

        Events.DoubleJumpCardActivated += jump; //EventListener, et kui Event scriptis DoubleJumpCardActivated invokitakse, siis ta h�ppaks
        Events.DashCardActivated += dash;
        Events.OnPlayerDead += setDead;
        Events.UltraDashCardActivated += ultraDash;
        Events.OnGetMovementDisabled += getMovementDisabled;
        Events.OnGetPlayerOnGround += onGround;
        Events.OnSetMovementDisabled += setMovementDisabled;
        Events.WallJumpCardActivated += wallJump;
        Events.HookshotCardActivated += SetGrapplePoint;
        Events.OnGetIsGrappling += getIsGrappling;
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
        Events.WallJumpCardActivated -= wallJump;
        Events.HookshotCardActivated -= SetGrapplePoint;
        Events.OnGetIsGrappling -= getIsGrappling;
    }

    void Update()
    {

        horizontalMovement = getMovementInput();

        movementLogic();                // Liikumise loogika siin sees
        rb.velocity = new Vector2(speed, rb.velocity.y);        // Liigutame uute asukohta

        checkDeath();       // Igal kaadril vaatame kas tegelane on surnud ja maas.

        jumpLogic();

        wallSlideLogic();       // Peab olema peale jump logicut for reasons

        grappleLogic();           // Kuna praegu lic ei tööta, siis get it outta here

        dashLogic();

        ultraDashLogic();

        switchSides();              // Muudame liikumise suunda kui tarvis.

        if (!isWallSliding) //Wallslide on erinev
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, facingRight ? 0f : 180f, 0f));       // P��rame �mber vastavalt facingRight booleanile.
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

    private bool getGrappleButtonDown()
    {
        if (getMovementDisabled())
        {
            return false;
        }
        return Input.GetButtonDown("Fire1");
    }

    private void wallSlideLogic()
    {
        if (isFalling && checkWall(facingRight) && !ActivatedWallJump)
        {
            if (facingRight)
            {
                if (getMovementInput() == 1)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                    isWallSliding = true;
                    rb.velocity = new Vector2(0, -wallSlideSpeed);
                }
                else
                {
                    isWallSliding = false;
                    animator.SetBool("isWallSliding", false);
                }
            }
            else
            {
                if (getMovementInput() == -1)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    isWallSliding = true;
                    rb.velocity = new Vector2(0, -wallSlideSpeed);
                }
                else
                {
                    isWallSliding = false;
                    animator.SetBool("isWallSliding", false);
                }
            }
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("isWallSliding", false);
        }
    }

    private void jumpLogic()
    {
        if (getJumpButtonDown() && onGround())    // Kui vajutad h�ppamist ja oled maas, siis h�ppad.
        {
            jump(false);
            Events.PlaySound("Jump");
        }
        if (getJumpButtonUp() && rb.velocity.y > 0f)     // Kui lased varem lahti on h�pe n�rgem.  Saaks teha ka gravitatsiooni muutmisega, mis on pehmem(?) pmst.
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (onGround())
        {
            animator.SetBool("Landing", true);
        }
        else
        {
            animator.SetBool("Landing", false);     // Landing animation thing
        }

        if (rb.velocity.y == 0f && !isDashing && !isUltraDashing)         // Kui oled maa peal. siis tagasi �igele gravitatsioonile.
        {
            if (rb.gravityScale != gravityMultiplier && !isGrappling) Events.PlaySound("Landing");
            rb.gravityScale = gravityMultiplier;
            isFalling = false;
            animator.SetBool("isFalling", false);
        }
        if (rb.velocity.y < 0f && !isDashing && !isUltraDashing)             // Kui hakkad h�ppel kukkuma, on gravitatsioon suurem.
        {
            rb.gravityScale = gravityMultiplier * 1.5f;
            isFalling = true;

            animator.SetBool("isJumping", false);

            if (isWallSliding)      // Wallsliding animation stuff
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isWallSliding", true);
            }
            else
            {
                animator.SetBool("isFalling", true);
            }
        }
    }

    private void jump(bool cardActivation)
    {
        if (cardActivation)
        {
            rb.gravityScale = gravityMultiplier;    // Sätime double jumpiks gravitatsiooni tavaliseks tagasi
            ActivatedDoubleJump = true;
            Events.PlaySound("Jump");
            StartCoroutine(CheckUntilPlayerOnGroundAfterDoubleJump()); //Alustab Coroutine'i p�rast double jump aktiveerimist.
        }

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        animator.SetBool("isJumping", true);
    }

    private void wallJump()
    {
        isWallSliding = false;
        ActivatedWallJump = true;

        rb.gravityScale = gravityMultiplier;    // Sätime wall jumpiks gravitatsiooni tavaliseks tagasi
        Events.PlaySound("Jump");
        if (facingRight)
        {
            rb.velocity = new Vector2(-wallJumpDistance, wallJumpPower);
            facingRight = false;
        }
        else
        {
            rb.velocity = new Vector2(wallJumpDistance, wallJumpPower);
            facingRight = true;
        }
        

        animator.SetBool("isJumping", true);

        Invoke("WallJumpReset",0.1f);
        DisableMovementFor(0.1f);
    }

    private void WallJumpReset()
    {
        ActivatedWallJump = false;
    }

    private void grappleLogic()
    {
        if (isGrappling && !isDead)
        {
            if (launchToPoint)
            {
                rb.gravityScale = 0;

                Vector2 prevPos = this.transform.position;
                this.transform.position = Vector2.Lerp(this.transform.position, grapplePoint, Time.deltaTime * launchSpeed);       // Doomed shit idk

                RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size*0.92f, 0f, grapplePoint, 0.1f, Ground);   // Kontrollime, et ei läheks millelegi pihta.

                DrawRope();

                if (hit || Vector2.Distance(prevPos,this.transform.position)<0.005f)
                {
                    EndGrapple();
                }
            }
        }
    }

    public void SetGrapplePoint()
    {
        if (onGround())
        {
            if (main_camera.ScreenToWorldPoint(Input.mousePosition).y > rb.position.y + 0.1f)
            {
                Grapple();
            }
        }
        else
        {
            Grapple();
        }
    }

    public void Grapple()
    {
        Vector2 distanceVector = main_camera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;  // Leiame mis suunda lasta

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, maxDistance, 8);        // Otsime, kas grapple sai millelegi pihta
        if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance && Vector2.Distance(hit.point, firePoint.position) >= minDistance)
        {
            Events.PlaySound("Grapple");   // PEAB GRAPPLE SOUND PANEMA
            animator.SetBool("isGrappling", true);
            animator.SetBool("isJumping", false);
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;
            movementDisabled = true;
            rb.velocity = Vector2.zero;
        }
    }

    public void EndGrapple()
    {
        isGrappling = false;
        movementDisabled = false;
        lineRenderer.enabled=false;
        animator.SetBool("isGrappling", false);

        if (facingRight) rb.velocity = new Vector2(0, hopStrength);
        else rb.velocity = new Vector2(0, hopStrength);

        rb.gravityScale = gravityMultiplier;
    }


    private void dash(bool cardActivation)
    {
        if (cardActivation && !isDashing)
        {
            Events.PlaySound("Dash");
            isDashing = true;
            dashSetup();
            CancelInvoke("StopDashing");
        }
    }
    private void ultraDash(bool cardActivation)
    {
        if (cardActivation && !isUltraDashing)
        {
            Events.PlaySound("Dash");
            movementDisabled = true;
            isUltraDashing = true;
            Events.PlaySound("Dash");
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
        Events.PlaySound("Death");
        setMovementDisabled(true);
        animator.SetBool("isJumping", false);
        rb.simulated = false;
        animator.SetBool("isGrappling", false);
        isGrappling = false;
         
        this.GetComponent<BoxCollider2D>().enabled = false;
  
        isDead = true;
    }
    private void checkDeath()
    {
        if (isDead )
        {
            isDead = false;
            animator.SetTrigger("Dead");
            
        }
    }
   
    private bool getMovementDisabled()
    {
        return movementDisabled;
    }

    private bool getIsGrappling()
    {
        return isGrappling;
    }

    private void DisableMovementFor(float time)
    {
        movementDisabled = true;
        Invoke("EnableMovement",time);
    }
    private void EnableMovement()
    {
        movementDisabled = false;
    }

    private void setMovementDisabled(bool disable)
    {
        movementDisabled = disable;
    }

    public void RestartLevelOnDeath()   // Selle kutsub death animation v�lja kui l�bi saab
    {
        Events.RestartLevel();
    }

    void DrawRope()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
        if (firePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(firePoint.position, minDistance);
        }
    }
}
