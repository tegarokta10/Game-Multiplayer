using UnityEngine;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{
    private PongGameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<PongGameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek jika bola mencapai goal Player 1 atau Player 2
        if (collision.CompareTag("GoalPlayer1"))
        {
            gameManager.AddPointToPlayer2();
            Destroy(gameObject); // Hancurkan bola setelah mencetak skor
        }
        else if (collision.CompareTag("GoalPlayer2"))
        {
            gameManager.AddPointToPlayer1();
            Destroy(gameObject); // Hancurkan bola setelah mencetak skor
        }
    }
}
