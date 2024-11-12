using UnityEngine;
using Unity.Netcode;

public class ControlPaddle : NetworkBehaviour
{
    public Transform paddle;
    public float moveSpeed = 5f;
    public float paddleYLimit = 3.62f;
    public int health = 5; // Health paddle
    public int maxHealth = 5;

    public GameObject ballPrefab;
    public Transform spawnPoint;
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode shootKey = KeyCode.Space;

    public float shootDirection = 1f; // 1 untuk kanan (positif X), -1 untuk kiri (negatif X)

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();

        if (Input.GetKeyDown(shootKey))
        {
            ShootBallServerRpc();
        }
    }

    private void HandleMovement()
    {
        // Menggerakkan paddle ke atas atau ke bawah sesuai input
        float moveDirection = 0f;
        if (Input.GetKey(moveUpKey))
        {
            moveDirection = 1f;
        }
        else if (Input.GetKey(moveDownKey))
        {
            moveDirection = -1f;
        }
        
        paddle.Translate(Vector2.up * moveDirection * moveSpeed * Time.deltaTime);
        
        // Membatasi posisi paddle agar tidak keluar dari batas Y yang ditentukan
        paddle.position = new Vector2(
            paddle.position.x,
            Mathf.Clamp(paddle.position.y, -paddleYLimit, paddleYLimit)
        );
    }

    [ServerRpc]
    private void ShootBallServerRpc()
    {
        Vector3 spawnPosition = spawnPoint.position;
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = new Vector2(shootDirection * 5f, 0); // Tembakkan bola hanya dalam arah horizontal
        }

        ball.GetComponent<NetworkObject>().Spawn();
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} terkena damage. Sisa Health: {health}");

        // Hancurkan paddle jika health habis dan hentikan permainan jika paddle milik host
        if (health <= 0)
        {
            Debug.Log($"{gameObject.name} telah dihancurkan!");
            Destroy(gameObject);

            if (IsHost)
            {
                Debug.Log("Permainan berakhir! Server host berhenti.");
                NetworkManager.Singleton.Shutdown();
            }
        }
    }
}
