using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private GameObject gamePanel;

    public Button hostButton;
    public Button clientButton;
    public Button readyButton;
    public TextMeshProUGUI statusText;

    private bool isReady = false;

    private void Start()
    {
        // Button listeners
        hostButton.onClick.AddListener(OnHostButtonPressed);
        clientButton.onClick.AddListener(OnClientButtonPressed);
        readyButton.onClick.AddListener(OnReadyButtonPressed);
        readyButton.interactable = false;

        // Panel initialization
        ShowConnectionPanel();

        // Network callback for client connections
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnHostButtonPressed()
    {
        NetworkManager.Singleton.StartHost();
        ShowWaitingPanel();
        statusText.text = "Host mode active. Waiting for players...";
        readyButton.interactable = true;
    }

    private void OnClientButtonPressed()
    {
        NetworkManager.Singleton.StartClient();
        ShowWaitingPanel();
        statusText.text = "Connected as Client. Waiting for Host...";
        readyButton.interactable = true;
    }

    private void OnReadyButtonPressed()
    {
        isReady = true;
        PlayerConfigurationManager.Instance.AddPlayerConfiguration(new PlayerConfiguration { IsReady = isReady });
        statusText.text = "Ready! Waiting for other player...";
        readyButton.interactable = false;
        CheckAllPlayersReady();
    }

    private void OnClientConnected(ulong clientId)
    {
        // Limit to two players
        if (NetworkManager.Singleton.ConnectedClients.Count > 2)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
    }

    private void CheckAllPlayersReady()
    {
        if (PlayerConfigurationManager.Instance.AllPlayersReady)
        {
            ShowGamePanel();
            FindObjectOfType<PongGameManagerModul>().StartGame();
        }
    }

    private void ShowConnectionPanel()
    {
        connectionPanel.SetActive(true);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    private void ShowWaitingPanel()
    {
        connectionPanel.SetActive(false);
        waitingPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    private void ShowGamePanel()
    {
        connectionPanel.SetActive(false);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}
