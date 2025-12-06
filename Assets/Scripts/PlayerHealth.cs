using System.Collections;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //DMG SFX
    public AudioSource audioPlayer;
    public AudioClip dmgSFX;
    public AudioClip deathSFX;
    public AudioClip deathHitSFX;
    public AudioClip offMapDeath;

    //so the sounds don't just spam (specifically falling off the map)
    private bool sfxPlayed = false;

    //Healthbar Image
    public Image healthImage;


    // Starting health value for the Player
    public int health = 100;

    // Amount of damage the Player takes when hit
    public int damageAmount = 25;

    // Reference to the Player's SpriteRenderer (used for flashing red)
    private SpriteRenderer spriteRenderer;

    //this will be used to cover the screen upon death
    public SpriteRenderer deathOverlay;

    private void Start()
    {
        healthImage.enabled = true;
        // Get the SpriteRenderer component attached to the Player
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //======== Update ========//

    void Update()
    {
        healthImage.fillAmount = health/100f;

        //if the player is below the map, call the death method (using tranforms)
        if(transform.position.y < -20)
        {
            if (!sfxPlayed)
            {
                sfxPlayed = true;
                audioPlayer.PlayOneShot(offMapDeath, 2);
                StartCoroutine(HoldDeath());
                
            }
            
        }
    }

    // Method to reduce health when damage is taken
    public void TakeDamage()
    {
        health -= damageAmount; // subtract damage amount
        StartCoroutine(BlinkRed()); // briefly flash red

        // If health reaches zero or below, call Die()
        if (health <= 0)
        {
            audioPlayer.PlayOneShot(deathHitSFX);
            StartCoroutine(HoldDeath());
        }
        else
        {
            //play different sfx if dying or not
            audioPlayer.PlayOneShot(dmgSFX);
        }
    }

    private IEnumerator HoldDeath()
    {
        deathOverlay.enabled = true;
        deathOverlay.sortingOrder = 5;
        deathOverlay.transform.rotation = new Quaternion(0, 0, 180f, 1);
        deathOverlay.transform.position = new Vector3(transform.position.x + 20, 0.0f, 0);
        Time.timeScale = 0.1f;
        for(int x = 0; x < 23; x++)
        {
            yield return new WaitForSeconds(0.001f);
            deathOverlay.transform.position = new Vector3(deathOverlay.transform.position.x, transform.position.y, 0);
            deathOverlay.transform.localScale = new Vector3(deathOverlay.transform.localScale.x + 5f, deathOverlay.transform.localScale.y);
        }
        Time.timeScale = 1;
        yield return new WaitWhile(() => audioPlayer.isPlaying);
        yield return new WaitForSeconds(0.25f);
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
        SceneManager.LoadScene("MainMenu");
    }
}
