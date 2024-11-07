using UnityEngine;
using Unity.Netcode;

public class PongGameManagerModul : NetworkBehaviour
{
    public GameObject playerPrefab; // Tidak perlu ballPrefab jika sudah ada di dalam playerPrefab
    private int player1Points = 0;
    private int player2Points = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count == 2)
            {
                StartGame();
            }
            else
            {
                Debug.Log("Waiting for more players to join...");
            }
        }
    }

    public void StartGame()
    {
        if (IsServer && NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            Debug.Log("Game Started!");
        }
    }

    [ServerRpc]
    public void AddPointToPlayer1ServerRpc()
    {
        player1Points++;
        Debug.Log($"Player 1 Score: {player1Points}");
    }

    [ServerRpc]
    public void AddPointToPlayer2ServerRpc()
    {
        player2Points++;
        Debug.Log($"Player 2 Score: {player2Points}");
    }
}
