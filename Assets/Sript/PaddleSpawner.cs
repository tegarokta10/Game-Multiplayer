using UnityEngine;
using Unity.Netcode;

public class PaddleSpawner : NetworkBehaviour
{
    public GameObject hostPaddlePrefab;
    public GameObject clientPaddlePrefab;
    private PongGameManager gameManager;
    private bool isMusicStarted = false; // Flag untuk memulai musik hanya sekali

    private void Start()
    {
        gameManager = FindObjectOfType<PongGameManager>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Spawn paddle untuk host dan client, set sesuai dengan ownership masing-masing
            SpawnPaddle(true, OwnerClientId); // Spawn paddle untuk host
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (clientId != OwnerClientId)
                {
                    SpawnPaddle(false, clientId); // Spawn paddle untuk client
                }
            }

            // Mulai musik in-game setelah paddle di-spawn
            if (!isMusicStarted)
            {
                InGameSound.Instance.PlayInGameMusic();
                isMusicStarted = true; // Mencegah musik dimulai lebih dari sekali
            }
        }
    }

    private void SpawnPaddle(bool isHost, ulong clientId)
    {
        // Pilih prefab paddle untuk host atau client
        GameObject selectedPaddlePrefab = isHost ? hostPaddlePrefab : clientPaddlePrefab;

        // Posisi spawn untuk host dan client
        Vector3 spawnPosition = isHost ? new Vector3(-7f, 0f, 0f) : new Vector3(7f, 0f, 0f);
        GameObject paddleInstance = Instantiate(selectedPaddlePrefab, spawnPosition, Quaternion.identity);

        NetworkObject networkObject = paddleInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(clientId); // Set ownership sesuai clientId
        }

        // Set instance paddle di PongGameManager untuk reference
        if (gameManager != null)
        {
            if (isHost)
            {
                gameManager.paddle1 = paddleInstance.GetComponent<ControlPaddle>(); // Host sebagai paddle1
            }
            else
            {
                gameManager.paddle2 = paddleInstance.GetComponent<ControlPaddle>(); // Client sebagai paddle2
            }
        }
    }
}
