using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MainMenu : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button exitButton;
    public InputField ipAddressInput; // Referensi untuk Input Field IP Address

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
        exitButton.onClick.AddListener(ExitGame);
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    void StartClient()
    {
        // Ambil IP Address dari Input Field
        string ipAddress = ipAddressInput.text;
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipAddress, 7777); // Set port default ke 7777
        NetworkManager.Singleton.StartClient();
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
