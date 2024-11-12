using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class SpawnPlayerMenu : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject hostPaddlePrefab; // Assign your host paddle prefab here
    [SerializeField] private GameObject clientPaddlePrefab; // Assign your client paddle prefab here

    private bool isReady = false;

    private void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonPressed);
        clientButton.onClick.AddListener(OnClientButtonPressed);
        readyButton.onClick.AddListener(OnReadyButtonPressed);
        readyButton.interactable = false;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    public void OnHostButtonPressed()
    {
        if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StartHost();
            statusText.text = "Host mode active. Waiting for players...";
            readyButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("Host or Client is already running.");
            statusText.text = "Host or Client already running.";
        }
    }

    public void OnClientButtonPressed()
    {
        NetworkManager.Singleton.StartClient();
        statusText.text = "Connected as Client. Waiting for Host...";
        readyButton.interactable = true;
    }

    public void OnReadyButtonPressed()
    {
        isReady = true;
        PlayerConfigurationManager.Instance.AddPlayerConfiguration(new PlayerConfiguration { IsReady = isReady });
        statusText.text = "Ready! Waiting for other player...";
        readyButton.interactable = false;
        CheckAllPlayersReady();
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count > 2)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
    }

    private void CheckAllPlayersReady()
    {
        if (PlayerConfigurationManager.Instance.AllPlayersReady)
        {
            SpawnPaddles();
            FindObjectOfType<PongGameManagerModul>().StartGame();
        }
    }

    private void SpawnPaddles()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                SpawnPaddle(client.ClientId == NetworkManager.Singleton.LocalClientId, client.ClientId);
            }
        }
    }

    private void SpawnPaddle(bool isHost, ulong clientId)
    {
        GameObject paddlePrefab = isHost ? hostPaddlePrefab : clientPaddlePrefab;
        GameObject paddleInstance = Instantiate(paddlePrefab);

        NetworkObject networkObject = paddleInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(clientId);
        }
    }
}
