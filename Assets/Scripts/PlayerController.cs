using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables appear in the Inspector, so you can tweak them without editing code.
    public float moveSpeed = 4f;       // How fast the player moves left/right

    // Private variables are used internally by the script.
    private Rigidbody2D rb;            // Reference to the Rigidbody2D component
    private float timer = 0.0f;
    void Start()
    {
        // Grab the Rigidbody2D attached to the Player object once at the start.
        rb = GetComponent<Rigidbody2D>();
    }

    //ground check boolean for animations
    private bool isGrounded = false;
    void Update()
    {
        // if (isGrounded)
        // {
        //     Debug.Log("GROUNDED");
        // }
        // if (!isGrounded)
        // {
        //     Debug.Log("NOT GROUNDED");
        // }
        
        // --- Horizontal movement ---
        // Get input from keyboard (A/D or Left/Right arrows).
        float moveInput = Input.GetAxis("Horizontal");
        // Apply horizontal speed while keeping the current vertical velocity.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if(timer >= 0.65f)
        {
            //Debug.Log("TIMER DONE");
            if (Input.GetKeyDown("space"))
            {
                rb.linearVelocityY = 6.5f;
                //Debug.Log("JUMPED");

                timer = 0f;
            }
        }
    }

    // starts counting up if the player is on the ground
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            //Debug.Log("TIMER: " + timer);
            timer += Time.deltaTime;
            isGrounded = true;
        }        
        //Debug.Log(timer);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            isGrounded = false;
        }
    }
}
