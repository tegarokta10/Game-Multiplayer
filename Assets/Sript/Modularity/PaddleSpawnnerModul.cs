using UnityEngine;
using Unity.Netcode;

public class PaddleSpawnnerModul : NetworkBehaviour
{
    public GameObject hostPaddlePrefab; // Prefab paddle untuk Host
    public GameObject clientPaddlePrefab; // Prefab paddle untuk Client

    public override void OnNetworkSpawn()
    {
        // Jangan spawn paddle secara otomatis di sini
    }

    public void SpawnPaddlesForPlayers()
    {
        if (IsServer)
        {
            // Spawn paddle untuk Host
            SpawnPaddleServerRpc(true);
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
            networkObject.SpawnWithOwnership(isHost ? NetworkManager.Singleton.LocalClientId : rpcParams.Receive.SenderClientId);
        }
    }
}
