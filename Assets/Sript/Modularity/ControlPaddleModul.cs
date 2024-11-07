using UnityEngine;
using Unity.Netcode;

public class ControlPaddleModul : NetworkBehaviour
{
    private PlayerInputHandler playerInputHandler;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
            var config = new PlayerConfiguration
            {
                PlayerIndex = IsHost ? 0 : 1,
                IsReady = false
            };
            playerInputHandler.InitializePlayer(config);
            PlayerConfigurationManager.Instance.AddPlayerConfiguration(config);
        }
    }
}
