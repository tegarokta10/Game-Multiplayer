using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;

public class UINetworkIP : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI playersCount;
    [SerializeField] private TextMeshProUGUI ipAddressText;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private TMP_InputField ip;
    [SerializeField] private UnityTransport transport;

    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private NetworkVariable<int> readyPlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private bool isTransitioning = false;
    private string ipAddress = "0.0.0.0";

    private void Awake()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        readyButton.onClick.AddListener(OnReadyButtonClicked);

        readyButton.gameObject.SetActive(false); // Sembunyikan tombol Ready saat pertama kali dijalankan
        notificationText.text = "Silakan pilih Host atau Client untuk memulai.";
    }

    private void StartHost()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StartHost();
            ipAddress = GetLocalIPAddress();
            SetIpAddress(ipAddress);
            playersNum.Value = 1; // Host dihitung sebagai player pertama
            ipAddressText.text = $"Host IP: {ipAddress}";
            notificationText.text = "Host telah tersambung. Menunggu client...";

            // Memutar musik latar untuk lobby
            SoundManager.Instance.PlayMusic(SoundManager.Instance.lobbyMusic);
        }
    }

    private void StartClient()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost)
        {
            ipAddress = ip.text;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                SetIpAddress(ipAddress);
                NetworkManager.Singleton.StartClient();
                notificationText.text = "Menyambungkan ke Host...";
                // Memutar musik latar untuk lobby
                SoundManager.Instance.PlayMusic(SoundManager.Instance.lobbyMusic);
            }
            else
            {
                Debug.LogWarning("Masukkan alamat IP yang valid.");
            }
        }
        else
        {
            Debug.LogWarning("Client atau Host sudah berjalan.");
        }
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
            Debug.Log($"Client {clientId} connected. Total players: {playersNum.Value}");
            notificationText.text = "Client telah tersambung. Menunggu kedua pemain siap...";

            // Tampilkan tombol Ready jika kedua pemain terhubung
            if (playersNum.Value == 2)
            {
                readyButton.gameObject.SetActive(true);
                notificationText.text = "Kedua pemain terhubung. Silakan tekan Ready untuk memulai.";
            }
        }
        else
        {
            notificationText.text = "Berhasil tersambung ke Host. Silakan tekan Ready untuk memulai.";
            readyButton.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        playersCount.text = $"Players: {playersNum.Value}";

        // Update tombol Ready interaktif jika jumlah pemain siap kurang dari total pemain
        readyButton.interactable = readyPlayers.Value < playersNum.Value;
    }

    private void OnReadyButtonClicked()
    {
        if (IsServer)
        {
            readyPlayers.Value++;
            notificationText.text = "Host siap. Menunggu client siap...";
        }
        else
        {
            SubmitReadyServerRpc();
            notificationText.text = "Client siap. Menunggu host siap...";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitReadyServerRpc()
    {
        readyPlayers.Value++;
    }

    private void LateUpdate()
    {
        // Pastikan kedua pemain sudah siap sebelum transisi ke scene berikutnya
        if (IsServer && readyPlayers.Value == playersNum.Value && playersNum.Value == 2 && !isTransitioning)
        {
            StartCoroutine(DelaySceneTransition());
        }
    }

    private IEnumerator DelaySceneTransition()
    {
        isTransitioning = true;
        notificationText.text = "Kedua pemain siap. Pindah ke scene dalam 3 detik...";
        yield return new WaitForSeconds(3);
        // Ganti musik latar ke musik in-game sebelum pindah scene
        SoundManager.Instance.PlayMusic(SoundManager.Instance.inGameMusic);
        NetworkManager.SceneManager.LoadScene("InGame", LoadSceneMode.Single);

    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("Tidak ada adapter jaringan dengan alamat IPv4 di sistem!");
    }

    private void SetIpAddress(string newIpAddress)
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = newIpAddress;
    }
}
