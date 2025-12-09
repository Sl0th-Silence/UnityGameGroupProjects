using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public int coinsToGive = 1;
    private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = GameObject.FindWithTag("CoinText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.coins += coinsToGive;
            coinText.text = player.coins.ToString();
            Destroy(gameObject);
        }
    }

    //additional coins from powerups
    public void PowerUpCoins()
    {
        PlayerController player = gameObject.GetComponent<PlayerController>();
        coinText.text = player.coins.ToString();
    }
}
