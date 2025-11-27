using UnityEngine;

public class TakingDamage : MonoBehaviour
{

    // Detect collision with the Player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the colliding object is tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Add knockback when player hit
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

            // Access the PlayerHealth script and apply damage
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();

            //additional knockback on death for dramatic effect
            if(collision.gameObject.GetComponent<PlayerHealth>().health <= 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            }
        }
    }
}
