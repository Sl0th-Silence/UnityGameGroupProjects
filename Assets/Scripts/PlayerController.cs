using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int coins = 0;

    // Public variables appear in the Inspector, so you can tweak them without editing code.
    public float moveSpeed = 4f;       // How fast the player moves left/right
    public float jumpForce = 4f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    //SFX sources
    public AudioSource audioPlayer;
    public AudioClip walk;
    public AudioClip jumpGRND;
    public AudioClip jumpAIR;

    // Private variables are used internally by the script.
    private Rigidbody2D rb;      // Reference to the Rigidbody2D component
    
    //Bool for ground check
    private bool isGrounded;
    private bool doubleJump;
    private float timer = 0.0f;
    private float SFXTimer = 0.0f;
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

            audioPlayer.PlayOneShot(jumpGRND);
        }
        //if the player is pressing space & can double jump & is not on the ground
        else if(Input.GetKeyDown(KeyCode.Space) && doubleJump && !isGrounded)
        {
            //the double jump will be half the force of the normal jump
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce / 1.5f);
            doubleJump = false;
            audioPlayer.PlayOneShot(jumpAIR);
        }
        setAnimation(moveInput);//Call setAnimation function every frame to check which animations should be played
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        //if the player is touching the ground, we can allow them to double jump
        if (isGrounded)
        {
            doubleJump = true;
        }
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
                if(SFXTimer > 0.2586f)
                {
                    audioPlayer.PlayOneShot(walk);
                    SFXTimer = 0.0f;
                }
                else
                {
                    SFXTimer += Time.deltaTime;
                }
                
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
