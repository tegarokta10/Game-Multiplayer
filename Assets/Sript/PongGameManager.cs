using UnityEngine;
using Unity.Netcode;

public class PongGameManager : NetworkBehaviour
{
    public ControlPaddle paddle1;
    public ControlPaddle paddle2;
    public NetworkVariable<bool> gameWon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Update()
    {
        // Debugging untuk health paddle
        if (paddle1 != null && paddle2 != null)
        {
            Debug.Log($"P1 Health: {paddle1.health} | P2 Health: {paddle2.health}");
        }

        CheckGameStatus();
    }

    private void CheckGameStatus()
    {
        if (!IsServer || gameWon.Value || paddle1 == null || paddle2 == null) return;

        // Kondisi kemenangan
        if (paddle1.health.Value <= 0)
        {
            gameWon.Value = true;
            ShowWinNotificationClientRpc("Paddle 2 Menang!");
            SoundManager.Instance.StopMusic();
        }
        else if (paddle2.health.Value <= 0)
        {
            gameWon.Value = true;
            ShowWinNotificationClientRpc("Paddle 1 Menang!");
            SoundManager.Instance.StopMusic();
        }
    }

    [ClientRpc]
    private void ShowWinNotificationClientRpc(string message)
    {
        FindObjectOfType<UIGame>().DisplayWinNotification(message);
        FindObjectOfType<UIGame>().ShowRestartButton();
    }
}
