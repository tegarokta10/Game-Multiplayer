using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkManagerSetup : MonoBehaviour
{
    private void Awake()
    {
        var networkManager = NetworkManager.Singleton;

        if (networkManager != null && networkManager.NetworkConfig.NetworkTransport == null)
        {
            // Tambahkan UnityTransport jika tidak ada transport yang ditetapkan
            var unityTransport = gameObject.AddComponent<UnityTransport>();
            networkManager.NetworkConfig.NetworkTransport = unityTransport;
        }
    }
}
