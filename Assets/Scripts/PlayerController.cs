using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Coin))]
[RequireComponent(typeof(PlayerHealth))]
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
    public AudioClip boing;

    // Private variables are used internally by the script.
    private Rigidbody2D rb;      // Reference to the Rigidbody2D component
    private float lastDirection;
    private SpriteRenderer spriteRenderer;

    //Bool for ground check
    private bool isGrounded;
    private bool doubleJump;
    private float SFXTimer = 0.0f;
    private Animator animator; // Reference to Animator component

    //increasing jump height
    private bool increaseJump = false;
    private float increaseJumpTimer = 0.0f;

    void Start()
    {
        // Grab the Rigidbody2D attached to the Player object once at the start.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- Horizontal movement ---
        // Get input from keyboard (A/D or Left/Right arrows).
        float moveInput = Input.GetAxis("Horizontal");
        
        //Flipping char
        lastDirection = moveInput;
        if(lastDirection > 0)
        {
            //Right
            spriteRenderer.flipX = false;
        }
        else if(lastDirection < 0)
        {
            //Left
            spriteRenderer.flipX = true;
        }

            // Apply horizontal speed while keeping the current vertical velocity.
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

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

        //resets increased jump height 
        if(increaseJump && increaseJumpTimer > 2.5f)
        {
            //has been more than 2.5 seconds
            increaseJump = false;
            jumpForce -= 3.5f;
        }
        else
        {
            increaseJumpTimer += Time.deltaTime;
        }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BouncePad")
        {
            //Apply a stronger upward velocity when hitting the bounce pad
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 2f);

            //Play sound effect
            audioPlayer.PlayOneShot(boing);
        }
        else if(collision.gameObject.tag == "Strawberry")
        {
            //strawberry allows an extra jump
            doubleJump = true;

            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Pear")
        {
            //pears give an extra life
            PlayerHealth health = GetComponent<PlayerHealth>();
            health.GrabbedFruit();

            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Orange")
        {
            //oranges gives 15 coins
            coins += 15;

            Coin coinOBJ = GetComponent<Coin>();
            coinOBJ.PowerUpCoins();

            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Banana")
        {
            //banana increases jump force for a few seconds
            jumpForce += 3.5f;

            increaseJump = true;
            increaseJumpTimer = 0.0f;

            Destroy(collision.gameObject);
        }
    }

}
