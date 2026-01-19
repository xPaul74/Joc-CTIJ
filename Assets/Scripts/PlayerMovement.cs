using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [Header("Jumping Settings")] 
    public float jumpForce = 10f;
    public int maxExtraJumps = 1;

    [Header("Ground Check")] 
    public Transform groundCheck;   
    public LayerMask groundLayer; 
    public float checkRadius = 0.2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.25f;
    private float coyoteTimeCounter;

    [Header("Jump Buffering")]
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumps;
    bool jumpExecutedThisFrame = false;

    [Header("Dash Settings")]
    public float dashVelocity = 20f;
    public float dashTime = 0.3f;
    public float dashCooldown = 1f;
    public bool canDash = true;
    public bool isDashing;

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip jumpSound;    

    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = maxExtraJumps;

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Animator Component not found!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        jumpExecutedThisFrame = false;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.512f, 0.05f), 0f, groundLayer);

        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded)
        {
            extraJumps = maxExtraJumps;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
            anim.SetBool("isGrounded", isGrounded);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(PerformDash());
            }

        if (isDashing) return;

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime; 
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime; 
        }

        

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                audioSource.PlayOneShot(jumpSound);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;

                jumpExecutedThisFrame = true; 
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !jumpExecutedThisFrame && !isGrounded && extraJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            extraJumps--; 
            jumpBufferCounter = 0f;
            audioSource.PlayOneShot(jumpSound);
        }

        if (horizontalInput != 0)
        {
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        

    }
    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(transform.localScale.x * dashVelocity, 0f);

        if (anim != null) anim.SetBool("isDashing", true);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
        if (anim != null) anim.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void ResetMovementOnRespawn()
    {
        isDashing = false;
        canDash = true;

        if (anim != null)
        {
            anim.SetBool("isDashing", false);
            anim.Play("Player_Idle"); 
        }
    }
}
