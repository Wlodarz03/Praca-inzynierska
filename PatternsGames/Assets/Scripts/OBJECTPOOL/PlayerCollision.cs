using System.Security.Cryptography;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void Start()
    {
        GameManagerOP.Instance.onPlay.AddListener(ActivatePlayer);
    }
    private void ActivatePlayer()
    {
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Spike" || collision.transform.tag == "Projectile" || collision.transform.tag == "Row" || collision.transform.tag == "Col")
        {
            gameObject.SetActive(false);
            GameManagerOP.Instance.GameOver();
        }
    }
}
