using UnityEngine;
using Unity.Netcode;

public class PongGameManager : NetworkBehaviour
{
    public ControlPaddle paddle1;
    public ControlPaddle paddle2;

    private void Update()
    {
        // Debug.Log untuk menampilkan health terkini dari setiap paddle
        if (paddle1 != null && paddle2 != null)
        {
            Debug.Log($"Health Paddle 1: {paddle1.health} | Health Paddle 2: {paddle2.health}");
        }

        CheckGameStatus();
    }

    public void CheckGameStatus()
    {
        if (paddle1 != null && paddle2 != null)
        {
            if (paddle1.health <= 0 || paddle2.health <= 0)
            {
                Debug.Log("Permainan selesai!");

                // Kondisi untuk menentukan siapa yang menang atau tindakan lain
                if (paddle1.health <= 0)
                {
                    Debug.Log("Paddle 2 menang!");
                }
                else if (paddle2.health <= 0)
                {
                    Debug.Log("Paddle 1 menang!");
                }

                // Jika host, berhenti jika salah satu paddle health-nya 0
                if (IsHost)
                {
                    NetworkManager.Singleton.Shutdown();
                }
            }
        }
    }
}
