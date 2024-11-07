using UnityEngine;
using Unity.Netcode;

public class BallControllerModul : NetworkBehaviour
{
    private PongGameManagerModul gameManager;
    private Rigidbody2D rb;

    private void Start()
    {
        gameManager = FindObjectOfType<PongGameManagerModul>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitializeBall(bool isPlayer1)
    {
        Vector2 shootDirection = isPlayer1 ? Vector2.right : Vector2.left;
        rb.velocity = shootDirection * 5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal1"))
        {
            gameManager.AddPointToPlayer2ServerRpc(); // Tambah skor Player 2
            DestroyBall();
        }
        else if (collision.CompareTag("Goal2"))
        {
            gameManager.AddPointToPlayer1ServerRpc(); // Tambah skor Player 1
            DestroyBall();
        }
    }

    private void DestroyBall()
    {
        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
