using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class SpawnPlayerMenu : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button readyButton;
    public TextMeshProUGUI statusText;

    private bool isReady = false;
    private const int maxPlayers = 2; // Batas maksimal pemain

    private void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonPressed);
        clientButton.onClick.AddListener(OnClientButtonPressed);
        readyButton.onClick.AddListener(OnReadyButtonPressed);
        readyButton.interactable = false;

        // Callback untuk memeriksa jika ada pemain yang terhubung
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnHostButtonPressed()
    {
        NetworkManager.Singleton.StartHost();
        statusText.text = "Host mode active. Waiting for players...";
        readyButton.interactable = true;
    }

    private void OnClientButtonPressed()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count < maxPlayers)
        {
            NetworkManager.Singleton.StartClient();
            statusText.text = "Connected as Client. Waiting for Host...";
            readyButton.interactable = true;
        }
        else
        {
            statusText.text = "Max players reached. Cannot join as Client.";
        }
    }

    private void OnReadyButtonPressed()
    {
        isReady = true;

        // Tambahkan konfigurasi pemain ke PlayerConfigurationManager
        PlayerConfigurationManager.Instance.AddPlayerConfiguration(new PlayerConfiguration { IsReady = isReady });

        // Update UI status
        statusText.text = "Ready! Waiting for other player...";
        readyButton.interactable = false;

        // Cek apakah semua pemain siap, lalu mulai permainan jika iya
        CheckAllPlayersReady();
    }

    private void OnClientConnected(ulong clientId)
    {
        // Periksa jika sudah mencapai jumlah maksimal pemain
        if (NetworkManager.Singleton.ConnectedClients.Count > maxPlayers)
        {
            Debug.Log("Max players reached. Disconnecting extra players.");
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
        else
        {
            Debug.Log($"Player {clientId} connected");
        }
    }

    private void CheckAllPlayersReady()
    {
        if (PlayerConfigurationManager.Instance.AllPlayersReady)
        {
            PongGameManagerModul gameManager = FindObjectOfType<PongGameManagerModul>();
            if (gameManager != null)
            {
                gameManager.StartGame();
            }
        }
    }
}
