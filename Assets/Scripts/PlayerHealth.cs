using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //DMG SFX
    public AudioSource audioPlayer;
    public AudioClip dmgSFX;
    public AudioClip deathSFX;
    public AudioClip deathHitSFX;


    // Starting health value for the Player
    public int health = 100;

    // Amount of damage the Player takes when hit
    public int damageAmount = 25;

    // Reference to the Player's SpriteRenderer (used for flashing red)
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component attached to the Player
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Method to reduce health when damage is taken
    public void TakeDamage()
    {
        health -= damageAmount; // subtract damage amount
        StartCoroutine(BlinkRed()); // briefly flash red

        // If health reaches zero or below, call Die()
        if (health <= 0)
        {
            
            StartCoroutine(HoldUntilDeathSFXDone());
        }
        else
        {
            //play different sfx if dying or not
            audioPlayer.PlayOneShot(dmgSFX);
        }
    }

    private IEnumerator HoldUntilDeathSFXDone()
    {
        audioPlayer.PlayOneShot(deathHitSFX);
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.025f);
        audioPlayer.PlayOneShot(deathSFX);
        Time.timeScale = 0.35f;
        yield return new WaitForSeconds(0.182f);
        Time.timeScale = 1;
        Die();
    }

    // Coroutine to flash the Player red for 0.1 seconds
    private System.Collections.IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    // Reload the scene when the Player dies
    private void Die()
    {
        SceneManager.LoadScene("MainScene");
    }
}
