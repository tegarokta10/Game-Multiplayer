using UnityEngine;
using Unity.Netcode;

public class PaddleSpawner : NetworkBehaviour
{
    public GameObject hostPaddlePrefab; // Prefab paddle untuk Host
    public GameObject clientPaddlePrefab; // Prefab paddle untuk Client
    private PongGameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<PongGameManager>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Spawn paddle untuk Host
            SpawnPaddleServerRpc(true);
        }
        else
        {
            // Spawn paddle untuk Client
            SpawnPaddleServerRpc(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPaddleServerRpc(bool isHost, ServerRpcParams rpcParams = default)
    {
        // Pilih prefab berdasarkan apakah pemain adalah Host atau Client
        GameObject selectedPaddlePrefab = isHost ? hostPaddlePrefab : clientPaddlePrefab;
        GameObject paddleInstance = Instantiate(selectedPaddlePrefab, selectedPaddlePrefab.transform.position, selectedPaddlePrefab.transform.rotation);

        // Pastikan paddle memiliki NetworkObject agar bisa di-*spawn* di jaringan
        NetworkObject networkObject = paddleInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        }

        // Tetapkan paddle yang di-*spawn* ke paddle1 atau paddle2 di PongGameManager
        if (gameManager != null)
        {
            if (isHost)
            {
                gameManager.paddle1 = paddleInstance.GetComponent<ControlPaddle>();
            }
            else
            {
                gameManager.paddle2 = paddleInstance.GetComponent<ControlPaddle>();
            }
        }
    }
}
