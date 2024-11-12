using UnityEngine;
using Unity.Netcode;

public class PongGameManagerModul : NetworkBehaviour
{
    private int player1Points = 0;
    private int player2Points = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer && NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (IsServer)
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
