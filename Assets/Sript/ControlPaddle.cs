using UnityEngine;
using Unity.Netcode;

public class ControlPaddle : NetworkBehaviour
{
    public Transform paddle;
    public float moveSpeed = 5f;
    public float paddleYLimit = 3.62f;

    public GameObject ballPrefab;
    public Transform spawnPoint;
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode shootKey = KeyCode.Space;

    public Vector3 hostSpawnPosition = new Vector3(-8, 0, 0);
    public Vector3 clientSpawnPosition = new Vector3(8, 0, 0);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Atur posisi berdasarkan host atau client
            transform.position = IsHost ? hostSpawnPosition : clientSpawnPosition;

            // Rotasi 180 derajat pada sumbu Z untuk client
            if (!IsHost)
            {
                paddle.Rotate(0, 0, 180);
            }
        }
    }

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
        if (Input.GetKey(moveUpKey))
        {
            paddle.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(moveDownKey))
        {
            paddle.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }

        paddle.position = new Vector2(
            paddle.position.x,
            Mathf.Clamp(paddle.position.y, -paddleYLimit, paddleYLimit)
        );
    }

    [ServerRpc]
    private void ShootBallServerRpc()
    {
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = Vector2.right * 5f;
        }

        ball.GetComponent<NetworkObject>().Spawn();
    }
}
