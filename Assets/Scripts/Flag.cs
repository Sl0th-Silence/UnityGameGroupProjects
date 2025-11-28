using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject WinUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            WinUI.SetActive(true);
        }
    }
}
