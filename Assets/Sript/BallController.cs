using UnityEngine;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{
    public GameObject ballPlayer1Prefab; // Prefab bola untuk Player 1
    public GameObject ballPlayer2Prefab; // Prefab bola untuk Player 2
    private PongGameManager gameManager;
    public int damage = 1; // Damage yang dihasilkan oleh bola

    private void Start()
    {
        gameManager = FindObjectOfType<PongGameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek jika bola ini adalah milik Player 1 atau Player 2 berdasarkan prefab
        bool isPlayer1Ball = gameObject == ballPlayer1Prefab;
        bool isPlayer2Ball = gameObject == ballPlayer2Prefab;

        if (isPlayer1Ball && collision.gameObject.CompareTag("Paddle2"))
        {
            // Bola Player 1 hanya bisa mengurangi health Paddle 2
            var paddle = collision.gameObject.GetComponent<ControlPaddle>();
            if (paddle != null)
            {
                paddle.TakeDamage(damage);
            }
            Destroy(gameObject); // Hancurkan bola setelah tabrakan
        }
        else if (isPlayer2Ball && collision.gameObject.CompareTag("Paddle1"))
        {
            // Bola Player 2 hanya bisa mengurangi health Paddle 1
            var paddle = collision.gameObject.GetComponent<ControlPaddle>();
            if (paddle != null)
            {
                paddle.TakeDamage(damage);
            }
            Destroy(gameObject); // Hancurkan bola setelah tabrakan
        }
    }

    [ServerRpc]
    public void ShootBallServerRpc(bool isPlayer1, ServerRpcParams rpcParams = default)
    {
        // Pilih prefab bola berdasarkan pemain yang menembak
        GameObject selectedBallPrefab = isPlayer1 ? ballPlayer1Prefab : ballPlayer2Prefab;
        GameObject ballInstance = Instantiate(selectedBallPrefab, transform.position, Quaternion.identity);

        // Mengatur arah bola berdasarkan pemain yang menembak
        Rigidbody2D rb = ballInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Jika pemain 1, arahkan ke kanan (x positif), jika pemain 2, arahkan ke kiri (x negatif)
            float direction = isPlayer1 ? 1f : -1f;
            rb.velocity = new Vector2(5f * direction, 0); // Kecepatan awal bola
        }

        // Pastikan bola memiliki NetworkObject agar bisa di-*spawn* di jaringan
        NetworkObject networkObject = ballInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn();
        }
    }
}
