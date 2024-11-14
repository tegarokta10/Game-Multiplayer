using UnityEngine;
using Unity.Netcode;

public class ControlPaddle : NetworkBehaviour
{
    public Transform paddle;
    public float moveSpeed = 5f;
    public float paddleYLimit = 3.62f;
    public NetworkVariable<int> health = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public int maxHealth = 5;

    public GameObject ballPrefab;
    public Transform spawnPoint;
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode shootKey = KeyCode.Space;
    public float shootDirection = 1f;

    private float initialXPosition;

    void Start()
    {
        initialXPosition = paddle.position.x;
        if (IsServer)
        {
            health.Value = maxHealth;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();

        if (Input.GetKeyDown(shootKey))
        {
            ShootBall();

            // Panggil efek suara tembakan dari InGameSound
            InGameSound.Instance.PlayShootSFX();
        }
    }

    private void HandleMovement()
    {
        Vector2 moveDirection = new Vector2(
            (Input.GetKey(moveRightKey) ? 1 : 0) - (Input.GetKey(moveLeftKey) ? 1 : 0),
            (Input.GetKey(moveUpKey) ? 1 : 0) - (Input.GetKey(moveDownKey) ? 1 : 0)
        ).normalized;

        paddle.Translate(moveDirection * moveSpeed * Time.deltaTime);

        paddle.position = new Vector2(
            Mathf.Clamp(paddle.position.x, initialXPosition - 2f, initialXPosition + 2f),
            Mathf.Clamp(paddle.position.y, -paddleYLimit, paddleYLimit)
        );
    }

    private void ShootBall()
    {
        if (IsOwner)
        {
            SpawnBallServerRpc(spawnPoint.position, shootDirection);
        }
    }

    [ServerRpc]
    private void SpawnBallServerRpc(Vector3 position, float direction)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        var networkObject = ball.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn();
        }

        var rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(direction * 5f, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsServer)
        {
            health.Value -= damage;

            if (health.Value <= 0)
            {
                FindObjectOfType<UIGame>().ShowRestartButton();
                DestroyPaddleClientRpc();
            }
        }
    }

    [ClientRpc]
    private void DestroyPaddleClientRpc()
    {
        Destroy(gameObject);
    }

   public void ResetHealth()
    {
        if (IsServer)
        {
            health.Value = maxHealth;
        }
    }
}
