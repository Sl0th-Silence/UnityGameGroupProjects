using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables appear in the Inspector, so you can tweak them without editing code.
    public float moveSpeed = 4f;       // How fast the player moves left/right
    public float jumpForce = 4f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Private variables are used internally by the script.
    private Rigidbody2D rb;      // Reference to the Rigidbody2D component
    
    //Bool for ground check
    private bool isGrounded;
    private float timer = 0.0f;
    private Animator animator; // Reference to Animator component
    void Start()
    {
        // Grab the Rigidbody2D attached to the Player object once at the start.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Horizontal movement ---
        // Get input from keyboard (A/D or Left/Right arrows).
        float moveInput = Input.GetAxis("Horizontal");
        // Apply horizontal speed while keeping the current vertical velocity.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

            timer = 0f;
        }
        setAnimation(moveInput);//Call setAnimation function every frame to check which animations should be played
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void setAnimation(float moveInput)
    {
        if(isGrounded) //If player is touching the ground
        {
            if(moveInput==0) //If player is not moving, play idle animation
            {
                animator.Play("player_idle");
            }
            else
            {
                animator.Play("player_run"); // Otherwise play run animation
            }
        }
        else //If player is not touching the ground
        {
            if(rb.linearVelocity.y > 0) // If player is moving at upward velocity play jump animation
            {
                animator.Play("player_jump");
            }
            else
            {
                animator.Play("player_fall"); // Otherwise play fall animation
            }
        }
    }

}
