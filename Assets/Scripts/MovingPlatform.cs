using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float distance = 3f;     // How far the platform moves from its starting point
    public float speed = 2f;        // How fast the platform moves
    private Vector3 startPos;       // Store the starting position of the platform
    public int direction = 1;      // Direction of movement: 1 = right, -1 = left

    void Start()
    {
        startPos = transform.position; // Save the starting position when the game begins
    }

    void Update()
    {
        // Move the platform left/right each frame
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // If platform has moved the set distance from start
        if (Mathf.Abs(transform.position.x - startPos.x) >= distance)
        {
            // Reverse direction (switch between left and right)
            direction *= -1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player lands on the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Attach player to platform so they move together
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // If the player leaves the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detach player so they move independently again
            collision.transform.SetParent(null);
        }
    }
}