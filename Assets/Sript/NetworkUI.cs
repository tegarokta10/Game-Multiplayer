using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI playersCount;

    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private NetworkVariable<int> readyPlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    
    public bool isTranstioning = false;
    private bool isAllReady = false;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

        readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    private void Update()
    {
        if (IsServer)
        {
            playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
            Debug.Log($"Connected Players: {playersNum.Value}, Ready Players: {readyPlayers.Value}");

            if (readyPlayers.Value == 2)
            {
                isAllReady = true;
                Debug.Log("Both players are ready. Changing scene...");
                SceneTransition();
            }
        }

        playersCount.text = "Players : " + playersNum.Value.ToString();
        readyButton.interactable = playersNum.Value == 2;
    }

    private void OnReadyButtonClicked()
    {
        // Menambah jumlah pemain yang siap (hanya di server)
        if (IsServer)
        {
            readyPlayers.Value++;
        }
    }

    private void SceneTransition()
    {
        if (isAllReady && !isTranstioning)
        {
            Debug.Log("Transitioning to the next scene...");
            isTranstioning = true;
            NetworkManager.SceneManager.LoadScene("InGame", LoadSceneMode.Single);
        }
    }
}
